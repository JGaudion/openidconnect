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
using OpenIDConnect.Core.Utilities;

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
                //I think the role name should be unique...so check the rolename first

                if(roleManager.CheckRoleNameInUse(name))
                {
                    throw new Exception("Role name is already in use. Please choose a new role name");
                }

                var role = new Role()
                {
                    Name = name
                };

                //These seems to have to be done before the create, otherwise doesn't save (and I don't have access to context save?)
                //Ensure that we don't accidentally pick up the password claim!
                var SafeClaims = claims.Where(c => c.Type != Core.Constants.ClaimTypes.Password && c.Type != Core.Constants.ClaimTypes.Name);
                foreach (var claim in SafeClaims)
                {
                    TypeUtilities.SetProperty(role, claim.Type, claim.Value);
                }

                //Create a role 
                var result = await roleManager.CreateAsync(role);
                
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
                //Check the email address is unique
                if(manager.CheckEmailAlreadyInUse(claims))
                {
                    throw new Exception("This email is already in use by an existing user");//This will be turned into the UserManagementResult with the errors array
                }

                var name = !string.IsNullOrEmpty(username) ? username : Guid.NewGuid().ToString("N");
                var user = new User() {
                    UserName = name,
                    DisplayName = name,
                };
                //Create a user with the supplied password
                var result = await manager.CreateAsync(user, password);
                if(result.Errors.Any())
                {
                    return new UserManagementResult<string>(result.Errors);
                }

                //Deal with the claims
                if(manager.SupportsUserEmail)
                {
                    claims = await manager.SetAccountEmailAsync(user.Id, claims);
                }

                if(manager.SupportsUserPhoneNumber)
                {
                    claims = await manager.SetAccountPhoneAsync(user.Id, claims);
                }

                if (manager.SupportsUserClaim)
                {
                    claims = await manager.SetOtherClaims(user.Id, claims);
                }

                if (manager.SupportsUserRole)
                {
                    claims = await manager.SetRolesForUser(user.Id, claims);
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
           
                var createUserProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Name, "Name", true),
                new PropertyMetadata(PropertyTypes.Password, Core.Constants.ClaimTypes.Password, "Password", true),
                new PropertyMetadata(PropertyTypes.Email, Core.Constants.ClaimTypes.Email, "Email", true),
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Role, "Role", false)
            };

            var updateUserProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Name, "Name", true),
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Role, "Role", false)
            };

            var createRoleProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Name, "Name", true),
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Description, "Description", false)
            };

            var updateRoleProperties = new List<PropertyMetadata>
            {
                new PropertyMetadata(PropertyTypes.String, Core.Constants.ClaimTypes.Description, "Description", false)
            };

                var userMetadata = new UserMetadata(true, true, true, updateUserProperties, createUserProperties);
                var roleMetadata = new RoleMetadata(true, true, Core.Constants.ClaimTypes.Role, updateRoleProperties, createRoleProperties);

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
                    return new UserManagementResult<string>(new[] { "User not found" });
                }
                
            }
            catch (Exception e)
            {
                return new UserManagementResult<string>(new[] { e.Message });
            }
        }

        /// <summary>
        /// Assuming we are looking for a specific role and then changing the value of the property 
        /// by looking for the property with matching type
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<UserManagementResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(subject);
                if (role == null)
                {
                    return new UserManagementResult<string>(new[] { "Role not found" });
                }

                TypeUtilities.SetProperty(role, type, value);
                var test = role.Description;

                return UserManagementResult.Success;
            }
            catch (Exception e)
            {
                  return new UserManagementResult<UserDetail>(new[] { e.Message });
            }
        }

        /// <summary>
        /// Set the metadata on the property for the user, by looking for the property with matching type
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<UserManagementResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            try
            {
                var user = await manager.FindByIdAsync(subject);
                if(user == null)
                {
                    return new UserManagementResult<string>(new[] { "User not found" });
                }

                TypeUtilities.SetProperty(user, type, value);

                return UserManagementResult.Success;
            }
            catch (Exception e)
            {
                   return new UserManagementResult<string>(new[] { e.Message });
            }
        }
    }
}
