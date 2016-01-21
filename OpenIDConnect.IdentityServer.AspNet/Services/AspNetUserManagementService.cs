using OpenIDConnect.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIDConnect.Core.Models.UserManagement;
using System.Security.Claims;
using OpenIDConnect.IdentityServer.AspNet.Model;
using OpenIDConnect.Core.Constants;

namespace OpenIDConnect.IdentityServer.AspNet.Services
{
    public class AspNetUserManagementService : IUserManagementService
    {

        protected readonly UserManager manager;
        protected readonly RoleManager roleManager;
        public bool EnableSecurityStamp { get; set; }

        public AspNetUserManagementService(UserManager manager, RoleManager roleManager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            this.manager = manager;
            this.roleManager = roleManager;

            EnableSecurityStamp = true;
        }
        
        /// <summary>
        /// Assuming the subject is the equivalent of userid
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<UserManagementResult> AddUserClaimAsync(string subject, string type, string value)
        {
            try {

                var newClaim = new Claim(type, value);
                await manager.AddClaimAsync(subject, newClaim);

                return UserManagementResult.Success;
            }
            catch(Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }
        
        public async Task<UserManagementResult<string>> CreateRoleAsync(string roleName, IEnumerable<Claim> claims)
        {
            try
            {
                var name = !string.IsNullOrEmpty(roleName) ? roleName : Guid.NewGuid().ToString();
                var role = new Role()
                {
                    Name = name
                };
                //Create a role 
                var result = await roleManager.CreateAsync(role);

                //Deal with the claims
               // claims = await roleManager.SetClaims(name, claims);

                return new UserManagementResult<string>(name);
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult<string>> CreateUserAsync(string username, string password, IEnumerable<Claim> claims)
        {
            try
            {
                var name = !string.IsNullOrEmpty(username) ? username : Guid.NewGuid().ToString("N");
                var user = new User() {
                    UserName = name
                };
                //Create a user with the supplied password
                var result = await manager.CreateAsync(user, password);

                //Deal with the claims
                if(manager.SupportsUserEmail)
                {
                    claims = await manager.SetAccountEmailAsync(name, claims);
                }

                if(manager.SupportsUserPhoneNumber)
                {
                    claims = await manager.SetAccountPhoneAsync(name, claims);
                }

                if (manager.SupportsUserClaim)
                {
                    claims = await manager.SetOtherClaims(name, claims);
                }

                if (manager.SupportsUserRole)
                {
                    claims = await manager.SetRolesForUser(name, claims);
                }

                return new UserManagementResult<string>(name);
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult> DeleteRoleAsync(string subject)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(subject);
                if(role != null)
                {
                    await roleManager.DeleteAsync(role);
                }
                return UserManagementResult.Success;
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult> DeleteUserAsync(string subject)
        {
            try
            {
                var user = await manager.FindByIdAsync(subject);
                if(user != null)
                {
                    await manager.DeleteAsync(user);
                }
                return UserManagementResult.Success;
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        public Task<UserManagementMetadata> GetMetadataAsync()
        {
           
                var createProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Name, "Name", true),
                new PropertyMetadata(PropertyTypes.Password, Core.Constants.ClaimTypes.Password, "Password", true),
                new PropertyMetadata(PropertyTypes.Email, Core.Constants.ClaimTypes.Email, "Email", false)
            };

                var userMetadata = new UserMetadata(true, false, false, Enumerable.Empty<PropertyMetadata>(), createProperties);
                var roleMetadata = new RoleMetadata(false, false, Core.Constants.ClaimTypes.Role, Enumerable.Empty<PropertyMetadata>(), Enumerable.Empty<PropertyMetadata>());

                return Task.FromResult(new UserManagementMetadata(userMetadata, roleMetadata));  
        }

        public async Task<UserManagementResult<RoleDetail>> GetRoleAsync(string subject)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(subject);
                return new UserManagementResult<RoleDetail>(new RoleDetail(subject, role.Name, role.Description, Enumerable.Empty<Claim>()));
            }
            catch (Exception e)
            {
                return new UserManagementResult<RoleDetail>(new[] { e.Message });
            }
        }

        /// <summary>
        /// Why do we return empty list of claims?
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public async Task<UserManagementResult<UserDetail>> GetUserAsync(string subject)
        {
            try
            {
                var user = await manager.FindByIdAsync(subject);                
                return new UserManagementResult<UserDetail>(new UserDetail(subject, user.UserName, user.DisplayName, Enumerable.Empty<Claim>(), Enumerable.Empty<Claim>()));
            }
            catch (Exception e)
            {
                return new UserManagementResult<UserDetail>(new[] { e.Message });
            }
        }

        public async Task<UserManagementResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                throw new NotImplementedException("Not implemented user query with filter");
            }

            try
            {
                var result = await roleManager.QueryRolesAsync(start, count, filter);

                return new UserManagementResult<QueryResult<RoleSummary>>(roleManager.ToDomain(result, r => new RoleSummary(r.Id, r.Name, r.Description)));
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
                var result = await manager.QueryUsersAsync(start, count, filter);

                return new UserManagementResult<QueryResult<UserSummary>>(manager.ToDomain(result, r => new UserSummary(r.Id, r.UserName, r.DisplayName)));
            }
            catch (Exception e)
            {
                return new UserManagementResult<QueryResult<UserSummary>>(new[] { e.Message });
            }
        }

        /// <summary>
        /// I have assumed that this is removing a claim from a user
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<UserManagementResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            try
            {
                var user = manager.FindByIdAsync(subject);
                if(user != null)
                {
                    var userClaims = await manager.GetClaimsAsync(subject);
                    if (userClaims != null)
                    {
                        var claim = userClaims.Where(c => c.Type == type && c.Value == value).FirstOrDefault();
                        if (claim != null)
                        {
                            await manager.RemoveClaimAsync(subject, claim);
                        }
                    }
                    return UserManagementResult.Success;
                }
                else
                {
                    throw new Exception("User not found");
                }
                
            }
            catch (Exception e)
            {
                return new UserManagementResult<UserDetail>(new[] { e.Message });
            }
        }

        /// <summary>
        /// Assuming we are looking for a specific role and then changing the value of the property 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<UserManagementResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
            //try
            //{
            //    //awaiting code
            //}
            //catch (Exception e)
            //{
            //  //  return new UserManagementResult<UserDetail>(new[] { e.Message });
            //}
        }

        public Task<UserManagementResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
            //try
            //{
            //    //awaiting code
            //}
            //catch (Exception e)
            //{
            // //   return new UserManagementResult<UserDetail>(new[] { e.Message });
            //}
        }
    }
}
