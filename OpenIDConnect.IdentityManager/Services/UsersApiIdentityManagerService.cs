using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityManager;
using System.Net.Http;
using System.Text;
using OpenIDConnect.Core.Extensions;
using OpenIDConnect.IdentityManager.Dtos;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace OpenIDConnect.IdentityManager.Services
{
    public class UsersApiIdentityManagerService : IIdentityManagerService
    {
        private readonly string usersApiUri;

        public UsersApiIdentityManagerService(string usersApiUri)
        {
            this.usersApiUri = usersApiUri;
        }

        public Task<IdentityManagerMetadata> GetMetadataAsync()
        {
            var properties = new[] {
                new PropertyMetadata { DataType = PropertyDataType.String, Name = "User Name", Required = true, Type = Core.Constants.ClaimTypes.Name },
                new PropertyMetadata { DataType = PropertyDataType.Password, Name = "Password", Required = true, Type = Core.Constants.ClaimTypes.Password },
                new PropertyMetadata { DataType = PropertyDataType.Email, Name = "Email", Required = false, Type = Core.Constants.ClaimTypes.Email }
            };

            var meta = new IdentityManagerMetadata
            {
                UserMetadata = new UserMetadata
                {
                    SupportsClaims = true,
                    SupportsCreate = true,
                    SupportsDelete = true,
                    CreateProperties = properties,
                    UpdateProperties = properties
                },
                RoleMetadata = new RoleMetadata
                {
                    SupportsCreate = false,
                    SupportsDelete = false,
                    RoleClaimType = "role",
                    CreateProperties = Enumerable.Empty<PropertyMetadata>(),
                    UpdateProperties = Enumerable.Empty<PropertyMetadata>()
                }
            };

            return Task.FromResult(meta);
        }

        public async Task<IdentityManagerResult> AddUserClaimAsync(string subject, string type, string value)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            using (var postResponse = await client.PostAsync($"/api/users/{subject}/claims", new StringContent($"[{{ type: \"{type}\", value: \"{value}\" }}]", Encoding.Unicode, "text/json")))
            {
                if (!postResponse.IsSuccessStatusCode)
                {
                    return new IdentityManagerResult(new[] { $"Unable to add user claim, returned status code {postResponse.StatusCode}" });
                }
            }

            return IdentityManagerResult.Success;
        }

        public Task<IdentityManagerResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityManagerResult<UserDetail>> GetUserAsync(string subject)
        {
            UserDto userDto;
            IEnumerable<ClaimDto> claimDtos;

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            {
                using (var getResponse = await client.GetAsync($"/api/users/{subject}"))
                {
                    if (!getResponse.IsSuccessStatusCode)
                    {
                        return new IdentityManagerResult<UserDetail>(new[] { $"Unable to get user, returned status code {getResponse.StatusCode}" });
                    }
                    userDto = await getResponse.DeserializeJsonContentAsync<UserDto>();
                }

                using (var getResponse = await client.GetAsync($"/api/users/{subject}/claims"))
                {
                    if (!getResponse.IsSuccessStatusCode)
                    {
                        return new IdentityManagerResult<UserDetail>(new[] { $"Unable to get user claims, returned status code {getResponse.StatusCode}" });
                    }

                    claimDtos = await getResponse.DeserializeJsonContentAsync<IEnumerable<ClaimDto>>();
                }
            }

            var name = claimDtos.FirstOrDefault(c => c.Type == Core.Constants.ClaimTypes.Name)?.Value ?? userDto.Id;

            return new IdentityManagerResult<UserDetail>(
                new UserDetail
                {
                    Subject = userDto.Id,
                    Name = name,
                    Claims = claimDtos.Select(c => new ClaimValue { Type = c.Type, Value = c.Value })
                } );
        }

        public async Task<IdentityManagerResult<CreateResult>> CreateUserAsync(IEnumerable<PropertyValue> properties)
        {
            var userName = properties.FirstOrDefault(x => (x.Type == Core.Constants.ClaimTypes.Name || x.Type == Core.Constants.ClaimTypes.Username))?.Value ?? string.Empty;
            var password = properties.FirstOrDefault(x => x.Type == Core.Constants.ClaimTypes.Password)?.Value ?? string.Empty;

            var claims = properties.Where(x => x.Type != Core.Constants.ClaimTypes.Password).Select(x => new ClaimDto { Type = x.Type, Value = x.Value });

            var userCreateDto = new UserCreateDto { Username = userName, Password = password, Claims = claims };

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            {
                using (var postResponse = await client.PostAsync($"/api/users", new StringContent(JsonConvert.SerializeObject(userCreateDto), Encoding.Unicode, "text/json")))
                {
                    if (!postResponse.IsSuccessStatusCode)
                    {
                        return new IdentityManagerResult<CreateResult>(new[] { $"Error creating user, returned status code {postResponse.StatusCode}" });
                    }
                }
            }

            return new IdentityManagerResult<CreateResult>(new CreateResult { Subject = userName });
        }

        public async Task<IdentityManagerResult> DeleteUserAsync(string subject)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            using (var deleteResponse = await client.DeleteAsync($"/api/users/{subject}"))
            {
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    return new IdentityManagerResult(new[] { $"Error deleting user, returned status code {deleteResponse.StatusCode}" });
                }
            }

            return IdentityManagerResult.Success;
        }

        public async Task<IdentityManagerResult<QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            var filterParam = !string.IsNullOrEmpty(filter) ? $"filter={filter}" : string.Empty;
            var startParam = start != 0 ? $"{start}" : string.Empty;
            var countParam = count > 0 ? $"{count}" : string.Empty;

            var queryString = string.Join("&", new[] { filterParam, startParam, countParam }.Where(p => !string.IsNullOrEmpty(p)));
            queryString = string.IsNullOrEmpty(queryString) ? queryString : $"?{queryString}";

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            using (var getResponse = await client.GetAsync($"/api/users{queryString}"))
            {
                if (!getResponse.IsSuccessStatusCode)
                {
                    return new IdentityManagerResult<QueryResult<UserSummary>>(new[] { $"Error querying users, returned status code {getResponse.StatusCode}" });
                }

                var queryResult = await getResponse.DeserializeJsonContentAsync<QueryUsersResultDto>();

                return new IdentityManagerResult<QueryResult<UserSummary>>(
                    new QueryResult<UserSummary>
                    {
                        Count = queryResult.Count,
                        Filter = queryResult.Filter,
                        Start = queryResult.Start,
                        Items = queryResult.Users.Select(u => new UserSummary { Name = u.Id, Subject = u.Id, Username = u.Id }),
                        Total = queryResult.Total
                    });
            }
        }

        public Task<IdentityManagerResult<CreateResult>> CreateRoleAsync(IEnumerable<PropertyValue> properties)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> DeleteRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<RoleDetail>> GetRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }
    }
}
