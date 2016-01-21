using OpenIDConnect.Core.Models.UserManagement;
using OpenIDConnect.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimTypes = OpenIDConnect.Core.Constants.ClaimTypes;
using IM = IdentityManager;

namespace OpenIDConnect.IdentityManager.Services
{
    public class DomainIdentityManagerService : IM.IIdentityManagerService
    {
        private readonly IUserManagementService userManagementService;

        public DomainIdentityManagerService(IUserManagementService userManagementService)
        {
            this.userManagementService = userManagementService;
        }

        public async Task<IM.IdentityManagerResult> AddUserClaimAsync(string subject, string type, string value)
        {
            var result = await this.userManagementService.AddUserClaimAsync(subject, type, value);
            return new IM.IdentityManagerResult(result.Errors.ToArray());
        }

        public async Task<IM.IdentityManagerResult<IM.CreateResult>> CreateRoleAsync(IEnumerable<IM.PropertyValue> properties)
        {
            var roleName = properties.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            var result = await this.userManagementService.CreateRoleAsync(roleName, properties.Select(p => new Claim(p.Type, p.Value)));

            return ToIdentityManagerResult(result, r => new IM.CreateResult { Subject = r });
        }

        public async Task<IM.IdentityManagerResult<IM.CreateResult>> CreateUserAsync(IEnumerable<IM.PropertyValue> properties)
        {
            var userName = properties.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            var password = properties.FirstOrDefault(x => x.Type == ClaimTypes.Password)?.Value ?? string.Empty;
            var result = await this.userManagementService.CreateUserAsync(userName, password, 
                properties.Where(p => p.Value != null && p.Type != ClaimTypes.Password).Select(p => new Claim(p.Type, p.Value)));

            return ToIdentityManagerResult(result, r => new IM.CreateResult { Subject = r });

        }

        public async Task<IM.IdentityManagerResult> DeleteRoleAsync(string subject)
        {
            var result = await this.userManagementService.DeleteRoleAsync(subject);

            return ToIdentityManagerResult(result);
        }

        public async Task<IM.IdentityManagerResult> DeleteUserAsync(string subject)
        {
            var result = await this.userManagementService.DeleteUserAsync(subject);

            return ToIdentityManagerResult(result);
        }

        public async Task<IM.IdentityManagerMetadata> GetMetadataAsync()
        {
            var result = await this.userManagementService.GetMetadataAsync();

            var meta = ToIdentityManagerMetadata(result);

            return meta;
        }

        public async Task<IM.IdentityManagerResult<IM.RoleDetail>> GetRoleAsync(string subject)
        {
            var result = await this.userManagementService.GetRoleAsync(subject);

            return ToIdentityManagerResult(result, 
                r => new IM.RoleDetail
                {
                    Description = r.Description,
                    Name = r.Name,
                    Subject = r.Subject,
                    Properties = r.Claims.Select(p => new IM.PropertyValue { Type = p.Type, Value = p.Value })
                });

        }

        public async Task<IM.IdentityManagerResult<IM.UserDetail>> GetUserAsync(string subject)
        {
            var result = await this.userManagementService.GetUserAsync(subject);

            return ToIdentityManagerResult(result,
                r => new IM.UserDetail
                {
                    Name = r.Name,
                    Subject = r.Subject,
                    Username = r.Username,
                    Claims = r.Claims.Select(c => new IM.ClaimValue { Type = c.Type, Value = c.Value }),
                    Properties = r.Properties.Select(p => new IM.PropertyValue { Type = p.Type, Value = p.Value })
                });
        }

        public async Task<IM.IdentityManagerResult<IM.QueryResult<IM.RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            var result = await this.userManagementService.QueryRolesAsync(filter, start, count);

            return ToIdentityManagerResult(result,
                r => ToIdentityManagerQueryResult(r, 
                    s => new IM.RoleSummary
                    {
                        Description = s.Description,
                        Name = s.Name,
                        Subject = s.Subject
                    }));
        }

        public async Task<IM.IdentityManagerResult<IM.QueryResult<IM.UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            var result = await this.userManagementService.QueryUsersAsync(filter, start, count);

            return ToIdentityManagerResult(result,
                r => ToIdentityManagerQueryResult(r,
                    s => new IM.UserSummary
                    {
                        Name = s.Name,
                        Subject = s.Subject,
                        Username = s.Username
                    }));
        }

        public async Task<IM.IdentityManagerResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            var result = await this.userManagementService.RemoveUserClaimAsync(subject, type, value);

            return ToIdentityManagerResult(result);
        }

        public async Task<IM.IdentityManagerResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            var result = await this.userManagementService.SetRolePropertyAsync(subject, type, value);

            return ToIdentityManagerResult(result);
        }

        public async Task<IM.IdentityManagerResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            var result = await this.userManagementService.SetUserPropertyAsync(subject, type, value);

            return ToIdentityManagerResult(result);
        }

        private IM.QueryResult<TOut> ToIdentityManagerQueryResult<TIn, TOut>(QueryResult<TIn> result, Func<TIn, TOut> resultConverter)
        {
            return new IM.QueryResult<TOut>
            {
                Count = result.Count,
                Filter = result.Filter,
                Items = result.Items.Select(resultConverter),
                Start = result.Start,
                Total = result.Total
            };
        }

        private IM.IdentityManagerResult ToIdentityManagerResult(UserManagementResult result)
        {
            IM.IdentityManagerResult identityManagerResult;

            if (result.IsError)
            {
                identityManagerResult = new IM.IdentityManagerResult(result.Errors.ToArray());
            }
            else
            {
                identityManagerResult = IM.IdentityManagerResult.Success;
            }

            return identityManagerResult;
        }

        private IM.IdentityManagerResult<TOut> ToIdentityManagerResult<TIn, TOut>(UserManagementResult<TIn> result, Func<TIn, TOut> converter)
        {
            IM.IdentityManagerResult<TOut> identityManagerResult;

            if (result.IsError)
            {
                identityManagerResult = new IM.IdentityManagerResult<TOut>(result.Errors.ToArray());
            }
            else
            {
                identityManagerResult = new IM.IdentityManagerResult<TOut>(converter(result.Result));
            }

            return identityManagerResult;
        }

        private IM.IdentityManagerMetadata ToIdentityManagerMetadata(UserManagementMetadata result)
        {
            return new IM.IdentityManagerMetadata
            {
                UserMetadata = new IM.UserMetadata
                {
                    SupportsCreate = result.UserMetadata.SupportsCreate,
                    SupportsDelete = result.UserMetadata.SupportsDelete,
                    SupportsClaims = result.UserMetadata.SupportsClaims,
                    CreateProperties = result.UserMetadata.CreateProperties.Select(p => new IM.PropertyMetadata { DataType = ToDataType(p.DisplayFieldType), Type = p.ClaimType, Name = p.Name, Required = p.Required }),
                    UpdateProperties = result.UserMetadata.UpdateProperties.Select(p => new IM.PropertyMetadata { DataType = ToDataType(p.DisplayFieldType), Type = p.ClaimType, Name = p.Name, Required = p.Required })
                },
                RoleMetadata = new IM.RoleMetadata
                {
                    SupportsCreate = result.RoleMetadata.SupportsCreate,
                    SupportsDelete = result.RoleMetadata.SupportsDelete,
                    RoleClaimType = result.RoleMetadata.RoleClaimType,
                    CreateProperties = result.RoleMetadata.CreateProperties.Select(p => new IM.PropertyMetadata { DataType = ToDataType(p.DisplayFieldType), Type = p.ClaimType, Name = p.Name, Required = p.Required }),
                    UpdateProperties = result.RoleMetadata.UpdateProperties.Select(p => new IM.PropertyMetadata { DataType = ToDataType(p.DisplayFieldType), Type = p.ClaimType, Name = p.Name, Required = p.Required })
                }
            };
        }

        private IM.PropertyDataType ToDataType(string type)
        {
            return (IM.PropertyDataType)Enum.Parse(typeof(IM.PropertyDataType), type);
        }

    }
}
