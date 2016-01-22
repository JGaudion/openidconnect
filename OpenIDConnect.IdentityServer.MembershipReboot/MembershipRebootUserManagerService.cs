using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using IdentityManager;
using OpenIDConnect.Core.Constants;
using OpenIDConnect.Core.Models.UserManagement;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityServer.MembershipReboot.Extensions;
using ClaimTypes = OpenIDConnect.Core.Constants.ClaimTypes;
using PropertyMetadata = OpenIDConnect.Core.Models.UserManagement.PropertyMetadata;
using RoleDetail = OpenIDConnect.Core.Models.UserManagement.RoleDetail;
using RoleMetadata = OpenIDConnect.Core.Models.UserManagement.RoleMetadata;
using RoleSummary = OpenIDConnect.Core.Models.UserManagement.RoleSummary;
using UserDetail = OpenIDConnect.Core.Models.UserManagement.UserDetail;
using UserMetadata = OpenIDConnect.Core.Models.UserManagement.UserMetadata;
using UserSummary = OpenIDConnect.Core.Models.UserManagement.UserSummary;

namespace OpenIDConnect.IdentityServer.MembershipReboot
{
    public class MembershipRebootIdentityManagerService<TAccount, TGroup> : IUserManagementService
        where TAccount : UserAccount, new()
        where TGroup : Group, new()
    {
        private readonly UserAccountService<TAccount> _userAccountService;
        private readonly IUserAccountQuery<TAccount> _userQuery;
        private readonly GroupService<TGroup> _groupService;
        private readonly IGroupQuery _groupQuery;
        private readonly Func<Task<UserManagementMetadata>> _metadataFunc;

        public Func<IQueryable<TAccount>, string, IQueryable<TAccount>> Filter { get; set; }
        public Func<IQueryable<TAccount>, IQueryable<TAccount>> Sort { get; set; }

        public MembershipRebootIdentityManagerService(
           UserAccountService<TAccount> userAccountService,
           GroupService<TGroup> groupService,
           bool includeAccountProperties = true)
            : this(userAccountService, userAccountService.Query, groupService, groupService.Query, includeAccountProperties)
        {
        }

        public MembershipRebootIdentityManagerService(
            UserAccountService<TAccount> userAccountService,
            IUserAccountQuery<TAccount> userQuery,
            GroupService<TGroup> groupService,
            IGroupQuery groupQuery,
            bool includeAccountProperties = true)
        {
            if (userAccountService == null)
            {
                throw new ArgumentNullException(nameof(userAccountService));
            }

            if (userQuery == null)
            {
                throw new ArgumentNullException(nameof(userQuery));
            }

            _userAccountService = userAccountService;
            _userQuery = userQuery;

            _groupService = groupService;
            _groupQuery = groupQuery;

            _metadataFunc = () => Task.FromResult(GetStandardMetadata(includeAccountProperties));

            if (typeof(RelationalUserAccount).IsAssignableFrom(typeof(TAccount)))
            {
                Filter = RelationalUserAccountQuery<TAccount>.DefaultFilter;
                Sort = RelationalUserAccountQuery<TAccount>.DefaultSort;
            }
            else
            {
                Filter = DefaultFilter;
                Sort = DefaultSort;
            }
        }

        public MembershipRebootIdentityManagerService(
            UserAccountService<TAccount> userAccountService,
            IUserAccountQuery<TAccount> userQuery,
            GroupService<TGroup> groupService,
            IGroupQuery groupQuery,
            UserManagementMetadata metadata)
            : this(userAccountService, userQuery, groupService, groupQuery, () => Task.FromResult(metadata))
        {
        }

        public MembershipRebootIdentityManagerService(
            UserAccountService<TAccount> userAccountService,
            IUserAccountQuery<TAccount> userQuery,
            GroupService<TGroup> groupService,
            IGroupQuery groupQuery,
            Func<Task<UserManagementMetadata>> metadataFunc)
            : this(userAccountService, userQuery, groupService, groupQuery)
        {
            if (metadataFunc == null)
            {
                throw new ArgumentNullException(nameof(metadataFunc));
            }

            _metadataFunc = metadataFunc;
        }

        public UserManagementMetadata GetStandardMetadata(bool includeAccountProperties = true)
        {
            var update = new List<PropertyMetadata>();
            if (_userAccountService.Configuration.EmailIsUsername)
            {
                update.AddRange(new PropertyMetadata[]{
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Username, GetUsername, SetUsername, name: "Email", dataType: PropertyTypes.Email, required: true),
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Password, x => null, SetPassword, name: "Password", dataType: PropertyTypes.Password, required: true)
                });
            }
            else
            {
                update.AddRange(new PropertyMetadata[]{
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Username, GetUsername, SetUsername, name: "Username", dataType: PropertyTypes.String, required: true),
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Password, x => null, SetPassword, name: "Password", dataType: PropertyTypes.Password, required: true),
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Email, GetEmail, SetConfirmedEmail, name: "Email", dataType: PropertyTypes.Email, required: _userAccountService.Configuration.RequireAccountVerification)
                });
            }

            var create = new List<PropertyMetadata>();
            if (!_userAccountService.Configuration.EmailIsUsername && !_userAccountService.Configuration.RequireAccountVerification)
            {
                create.AddRange(update.Where(x => x.Required).ToArray());
                create.AddRange(new PropertyMetadata[]{
                    PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Email, GetEmail, SetConfirmedEmail, name: "Email", dataType: PropertyTypes.Email, required: false)
                });
            }

            update.AddRange(new PropertyMetadata[] {
                PropertyMetadata.FromFunctions<TAccount, string>(ClaimTypes.Phone, GetPhone, SetConfirmedPhone, name: "Phone", dataType: PropertyTypes.String, required: false),
                PropertyMetadata.FromFunctions<TAccount, bool>("IsLoginAllowed", GetIsLoginAllowed, SetIsLoginAllowed, name: "Is Login Allowed", dataType: PropertyTypes.Boolean, required: false)
            });

            if (includeAccountProperties)
            {
                update.AddRange(PropertyMetadata.FromType<TAccount>());
            }

            var userMetadata = new UserMetadata(true, true, true,
                createProperties: create,
                updateProperties: update);

            if (_groupService != null && _groupQuery != null)
            {
                var roleMetadata = new RoleMetadata(true, true, ClaimTypes.Role,
                createProperties: new[]{
                    new PropertyMetadata(
                        "Name",
                        ClaimTypes.Name,
                        PropertyTypes.String,
                        true)
                    },
                updateProperties: Enumerable.Empty<PropertyMetadata>());

                return new UserManagementMetadata(userMetadata, roleMetadata);
            }

            return new UserManagementMetadata(userMetadata, null);
        }

        public PropertyMetadata GetMetadataForClaim(string type, string name = null, PropertyDataType dataType = PropertyDataType.String, bool required = false)
        {
            return PropertyMetadata.FromFunctions<TAccount, string>(type, GetForClaim(type), SetForClaim(type), name, dataType, required);
        }

        public Func<TAccount, string> GetForClaim(string type)
        {
            return account => account.Claims.Where(x => x.Type == type).Select(x => x.Value).FirstOrDefault();
        }

        public Func<TAccount, string, UserManagementResult> SetForClaim(string type)
        {
            return (account, value) =>
            {
                try
                {
                    _userAccountService.RemoveClaim(account.ID, type);
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        _userAccountService.AddClaim(account.ID, type, value);
                    }
                }
                catch (ValidationException ex)
                {
                    return new UserManagementResult(new[] { ex.Message });
                }
                return UserManagementResult.Success;
            };
        }

        public string GetUsername(TAccount account)
        {
            if (_userAccountService.Configuration.EmailIsUsername)
            {
                return account.Email;
            }
            return account.Username;
        }

        public UserManagementResult SetUsername(TAccount account, string username)
        {
            try
            {
                if (_userAccountService.Configuration.EmailIsUsername)
                {
                    _userAccountService.SetConfirmedEmail(account.ID, username);
                }
                else
                {
                    _userAccountService.ChangeUsername(account.ID, username);
                }
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }

            return UserManagementResult.Success;
        }

        public UserManagementResult SetPassword(TAccount account, string password)
        {
            try
            {
                _userAccountService.SetPassword(account.ID, password);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }
            return UserManagementResult.Success;
        }

        public string GetEmail(TAccount account)
        {
            return account.Email;
        }

        public UserManagementResult SetConfirmedEmail(TAccount account, string email)
        {
            try
            {
                _userAccountService.SetConfirmedEmail(account.ID, email);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }
            return UserManagementResult.Success;
        }

        public string GetPhone(TAccount account)
        {
            return account.MobilePhoneNumber;
        }

        public UserManagementResult SetConfirmedPhone(TAccount account, string phone)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(phone))
                {
                    _userAccountService.RemoveMobilePhone(account.ID);
                }
                else
                {
                    _userAccountService.SetConfirmedMobilePhone(account.ID, phone);
                }
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }
            return UserManagementResult.Success;
        }

        public bool GetIsLoginAllowed(TAccount account)
        {
            return account.IsLoginAllowed;
        }

        public UserManagementResult SetIsLoginAllowed(TAccount account, bool value)
        {
            try
            {
                _userAccountService.SetIsLoginAllowed(account.ID, value);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }
            return UserManagementResult.Success;
        }

        public Task<UserManagementMetadata> GetMetadataAsync()
        {
            return _metadataFunc();
        }

        private static IQueryable<TAccount> DefaultFilter(IQueryable<TAccount> query, string filter)
        {
            return
                from acct in query
                where acct.Username.Contains(filter)
                select acct;
        }

        private static IQueryable<TAccount> DefaultSort(IQueryable<TAccount> query)
        {
            var result =
                from acct in query
                orderby acct.Username
                select acct;
            return result;
        }

        public Task<UserManagementResult<Core.Models.UserManagement.QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            if (start < 0)
            {
                start = 0;
            }

            if (count < 0)
            {
                count = Int32.MaxValue;
            }

            var filterFunc = Filter;
            if (String.IsNullOrWhiteSpace(filter))
            {
                filterFunc = (q, f) => q;
            }

            int total;
            var users = _userQuery.Query(query => filterFunc(query, filter), Sort, start, count, out total).ToArray();

            var result = new Core.Models.UserManagement.QueryResult<UserSummary>(
            start,
            count,
            total,
             filter,
            users.Select(x =>
                new UserSummary(
                    x.ID.ToString("D"),
                    x.Username,
                    DisplayNameFromUserId(x.ID)))
                .ToArray());

            return Task.FromResult(new UserManagementResult<Core.Models.UserManagement.QueryResult<UserSummary>>(result));
        }

        private string DisplayNameFromUserId(Guid id)
        {
            var acct = _userAccountService.GetByID(id);
            return acct.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
        }

        public async Task<UserManagementResult<string>> CreateUserAsync(string username, string password, IEnumerable<Claim> claims)
        {
            string[] exclude = { ClaimTypes.Username, ClaimTypes.Password, ClaimTypes.Email };
            var otherProperties = claims.Where(x => !exclude.Contains(x.Type)).ToArray();

            try
            {
                var metadata = await GetMetadataAsync();
                var createProps = metadata.UserMetadata.GetCreateProperties();

                var account = new TAccount();
                foreach (var prop in otherProperties)
                {
                    var result = SetUserProperty(createProps, account, prop.Type, prop.Value);
                    if (result.Errors.Any())
                    {
                        return new UserManagementResult<string>(result.Errors.ToArray());
                    }
                }

                if (_userAccountService.Configuration.EmailIsUsername)
                {
                    account = _userAccountService.CreateAccount(null, null, password, username, account: account);
                }
                else
                {
                    var emailClaim = claims.SingleOrDefault(x => x.Type == ClaimTypes.Email);
                    var email = emailClaim?.Value;

                    account = _userAccountService.CreateAccount(null, username, password, email, account: account);
                }

                return new UserManagementResult<string>(account.ID.ToString("D"));
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult<string>(ex.Message);
            }
        }

        public Task<UserManagementResult> DeleteUserAsync(string subject)
        {
            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return Task.FromResult(new UserManagementResult(new[] { "Invalid subject" }));
            }

            try
            {
                _userAccountService.DeleteAccount(g);
            }
            catch (ValidationException ex)
            {
                return Task.FromResult(new UserManagementResult(new[] { ex.Message }));
            }

            return Task.FromResult(UserManagementResult.Success);
        }

        public async Task<UserManagementResult<UserDetail>> GetUserAsync(string subject)
        {
            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return new UserManagementResult<UserDetail>(new[] { "Invalid subject" });
            }

            try
            {
                var account = _userAccountService.GetByID(g);
                if (account == null)
                {
                    return new UserManagementResult<UserDetail>((UserDetail)null);
                }

                var metadata = await GetMetadataAsync();

                var properties = metadata
                    .UserMetadata
                    .UpdateProperties
                    .Select(prop => new Claim(prop.ClaimType, GetUserProperty(prop, account))).ToList();

                var claims = new List<Claim>();
                if (account.Claims != null)
                {
                    claims.AddRange(account.Claims.Select(x => new Claim(x.Type, x.Value)));
                }

                var user = new UserDetail(
                    subject,
                    account.Username,
                    DisplayNameFromUserId(account.ID),
                    claims,
                    properties);

                return new UserManagementResult<UserDetail>(user);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult<UserDetail>(new[] { ex.Message });
            }
        }

        public async Task<UserManagementResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return new UserManagementResult(new[] { "Invalid subject" });
            }

            try
            {
                var acct = _userAccountService.GetByID(g);
                if (acct == null)
                {
                    return new UserManagementResult(new[] { "Invalid subject" });
                }

                var metadata = await GetMetadataAsync();
                var result = SetUserProperty(metadata.UserMetadata.UpdateProperties, acct, type, value);
                if (result.Errors.Any())
                {
                    return result;
                }

                try
                {
                    _userAccountService.Update(acct);
                }
                catch (ValidationException ex)
                {
                    return new UserManagementResult(new[] { ex.Message });
                }

                return UserManagementResult.Success;
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }
        }

        public Task<UserManagementResult> AddUserClaimAsync(string subject, string type, string value)
        {
            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return Task.FromResult(new UserManagementResult(new[] { "Invalid user." }));
            }

            try
            {
                _userAccountService.AddClaim(g, type, value);
            }
            catch (ValidationException ex)
            {
                return Task.FromResult(new UserManagementResult(new[] { ex.Message }));
            }

            return Task.FromResult(UserManagementResult.Success);
        }

        public Task<UserManagementResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return Task.FromResult(new UserManagementResult(new[] { "Invalid user." }));
            }

            try
            {
                _userAccountService.RemoveClaim(g, type, value);
            }
            catch (ValidationException ex)
            {
                return Task.FromResult(new UserManagementResult(new[] { ex.Message }));
            }

            return Task.FromResult(UserManagementResult.Success);
        }

        private static string GetUserProperty(PropertyMetadata propMetadata, TAccount user)
        {
            string val;
            if (propMetadata.TryGet(user, out val))
            {
                return val;
            }

            throw new Exception("Invalid property type " + propMetadata.ClaimType);
        }

        private static UserManagementResult SetUserProperty(IEnumerable<PropertyMetadata> propsMeta, TAccount user, string type, string value)
        {
            UserManagementResult result;
            if (propsMeta.TrySet(user, type, value, out result))
            {
                return result;
            }

            throw new Exception("Invalid property type " + type);
        }

        private void ValidateSupportsGroups()
        {
            if (_groupService == null || _groupQuery == null)
            {
                throw new InvalidOperationException("Groups Not Supported");
            }
        }

        public async Task<UserManagementResult<string>> CreateRoleAsync(string roleName, IEnumerable<Claim> claims)
        {
            ValidateSupportsGroups();

            var nameClaim = claims.Single(x => x.Type == ClaimTypes.Name);

            var name = nameClaim.Value;

            string[] exclude = { ClaimTypes.Name };
            var otherProperties = claims.Where(x => !exclude.Contains(x.Type)).ToArray();

            try
            {
                var metadata = await GetMetadataAsync();
                var createProps = metadata.RoleMetadata.GetCreateProperties();

                var group = _groupService.Create(name);
                foreach (var prop in otherProperties)
                {
                    var result = SetGroupProperty(createProps, group, prop.Type, prop.Value);
                    if (result.Errors.Any())
                    {
                        return new UserManagementResult<string>(result.Errors.ToArray());
                    }
                }

                try
                {
                    _groupService.Update(group);
                }
                catch (ValidationException ex)
                {
                    return new UserManagementResult<string>(new[] { ex.Message });
                }

                return new UserManagementResult<string>(group.ID.ToString("D"));
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult<string>(new[] { ex.Message });
            }
        }

        public Task<UserManagementResult> DeleteRoleAsync(string subject)
        {
            ValidateSupportsGroups();

            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return Task.FromResult(new UserManagementResult(new[] { "Invalid subject" }));
            }

            try
            {
                _groupService.Delete(g);
            }
            catch (ValidationException ex)
            {
                return Task.FromResult(new UserManagementResult(new[] { ex.Message }));
            }

            return Task.FromResult(UserManagementResult.Success);
        }

        public async Task<UserManagementResult<RoleDetail>> GetRoleAsync(string subject)
        {
            ValidateSupportsGroups();

            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return new UserManagementResult<RoleDetail>(new[] { "Invalid subject" });
            }

            try
            {
                var group = _groupService.Get(g);
                if (group == null)
                {
                    return new UserManagementResult<RoleDetail>((RoleDetail)null);
                }

                var metadata = await GetMetadataAsync();

                var properties = metadata
                    .RoleMetadata
                    .UpdateProperties
                    .Select(prop => new Claim(prop.ClaimType, GetGroupProperty(prop, @group))).ToList();


                var role = new RoleDetail(
                    subject,
                    group.Name,
                    String.Empty,
                    properties);

                return new UserManagementResult<RoleDetail>(role);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult<RoleDetail>(new[] { ex.Message });
            }
        }

        public Task<UserManagementResult<Core.Models.UserManagement.QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            ValidateSupportsGroups();

            if (start < 0) start = 0;
            if (count < 0) count = Int32.MaxValue;

            int total;
            var groups = _groupQuery.Query(filter, start, count, out total).ToArray();

            var result = new Core.Models.UserManagement.QueryResult<RoleSummary>(
            start,
            count,
            total,
             filter,
            groups.Select(x =>
                new RoleSummary(
                    x.ID.ToString("D"),
                    x.Name,
                    String.Empty)
                ).ToArray());

            return Task.FromResult(new UserManagementResult<Core.Models.UserManagement.QueryResult<RoleSummary>>(result));
        }

        public async Task<UserManagementResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            ValidateSupportsGroups();

            Guid g;
            if (!Guid.TryParse(subject, out g))
            {
                return new UserManagementResult(new[] { "Invalid subject" });
            }

            var group = _groupService.Get(g);
            if (group == null)
            {
                return new UserManagementResult(new[] { "Invalid subject" });
            }

            var metadata = await GetMetadataAsync();
            var result = SetGroupProperty(metadata.RoleMetadata.UpdateProperties, group, type, value);
            if (result.Errors.Any())
            {
                return result;
            }

            try
            {
                _groupService.Update(group);
            }
            catch (ValidationException ex)
            {
                return new UserManagementResult(new[] { ex.Message });
            }

            return UserManagementResult.Success;
        }

        private static string GetGroupProperty(PropertyMetadata propMetadata, TGroup group)
        {
            string val;
            if (propMetadata.TryGet(group, out val))
            {
                return val;
            }

            throw new Exception("Invalid property type " + propMetadata.ClaimType);
        }

        private static UserManagementResult SetGroupProperty(IEnumerable<PropertyMetadata> propsMeta, TGroup group, string type, string value)
        {
            UserManagementResult result;
            if (propsMeta.TrySet(group, type, value, out result))
            {
                return result;
            }

            throw new Exception("Invalid property type " + type);
        }
    }
}
