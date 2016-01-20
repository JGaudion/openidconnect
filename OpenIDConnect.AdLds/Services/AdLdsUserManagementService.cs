using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using OpenIDConnect.Core.Models.UserManagement;
using OpenIDConnect.Core.Services;
using OpenIDConnect.AdLds.Contexts;
using System.Linq;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.Core.Constants;
using ClaimTypes = OpenIDConnect.Core.Constants.ClaimTypes;

namespace OpenIDConnect.AdLds.Services
{
    public class AdLdsUserManagementService : IUserManagementService
    {
        private readonly IDirectoryContext directoryContext;

        public AdLdsUserManagementService(IDirectoryContextFactory contextFactory, DirectoryConnectionConfig connectionConfig)
        {
            this.directoryContext = contextFactory.CreateDirectoryContext(connectionConfig);
        }

        public async Task<UserManagementResult<string>> CreateUserAsync(string username, string password, IEnumerable<Claim> claims)
        {
            try
            {
                var result = await this.directoryContext.CreateUserAsync(username, password, claims);
                return new UserManagementResult<string>(result);
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult<UserDetail>> GetUserAsync(string subject)
        {
            try
            {
                var user = await this.directoryContext.FindUserByNameAsync(subject);
                return new UserManagementResult<UserDetail>(new UserDetail(subject, user.UserName, user.DisplayName, Enumerable.Empty<Claim>(), Enumerable.Empty<Claim>()));
            }
            catch (Exception e)
            {
                return new UserManagementResult<UserDetail>(new[] { e.Message });
            }
        }

        public Task<UserManagementResult> AddUserClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementResult<string>> CreateRoleAsync(string roleName, IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementResult> DeleteRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementResult> DeleteUserAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementMetadata> GetMetadataAsync()
        {
            var createProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, ClaimTypes.Name, "Name", true),
                new PropertyMetadata(PropertyTypes.Password, ClaimTypes.Password, "Password", true),
                new PropertyMetadata(PropertyTypes.Email, ClaimTypes.Email, "Email", false)
            };

            var userMetadata = new UserMetadata(true, false, false, Enumerable.Empty<PropertyMetadata>(), createProperties);
            var roleMetadata = new RoleMetadata(false, false, ClaimTypes.Role, Enumerable.Empty<PropertyMetadata>(), Enumerable.Empty<PropertyMetadata>());

            return Task.FromResult(new UserManagementMetadata(userMetadata, roleMetadata));
        }

        public Task<UserManagementResult<RoleDetail>> GetRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public async Task<UserManagementResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                throw new NotImplementedException("Not implemented role query with filter");
            }

            try
            {
                var result = await this.directoryContext.QueryRolesAsync(start, count);

                return new UserManagementResult<QueryResult<RoleSummary>>(ToDomain(result, r => new RoleSummary(r.Subject, r.Name, r.Description)));
            }
            catch (Exception e)
            {
                return new UserManagementResult<QueryResult<RoleSummary>>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult<QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                throw new NotImplementedException("Not implemented user query with filter");
            }

            try
            {
                var result = await this.directoryContext.QueryUsersAsync(start, count);

                return new UserManagementResult<QueryResult<UserSummary>>(ToDomain(result, r => new UserSummary(r.Id, r.UserName, r.DisplayName)));
            }
            catch (Exception e)
            {
                return new UserManagementResult<QueryResult<UserSummary>>(new[] { e.Message });
            }
        }

        public Task<UserManagementResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<UserManagementResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        private QueryResult<TOut> ToDomain<TIn, TOut>(QueryResult<TIn> result, Func<TIn, TOut> resultConverter)
        {
            return new QueryResult<TOut>(result.Start, result.Count, result.Total, result.Filter, result.Items.Select(resultConverter));
        }
    }
}
