using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenIDConnect.Authorization.Domain.Models;
using OpenIDConnect.Authorization.Domain.Repositories;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using OpenIDConnect.Core.Domain.Models;
using OpenIDConnect.Authorization.Data.UsersApi.Models;
using System.Linq;

namespace OpenIDConnect.Authorization.Data.UsersApi.Repositories
{
    public class UsersApiClientUsersRepository : IClientUsersRepository
    {
        private readonly Uri usersApiUri;

        public UsersApiClientUsersRepository(string usersApiUri)
        {
            this.usersApiUri = new Uri(new Uri(usersApiUri), "api/");
        }

        public async Task AddUserToGroup(string clientId, string groupId, User user)
        {
            var newClaim = new Claim("group", $"{clientId}:{groupId}");

            using (var client = CreateClient())
            using (var postResponse = await client.PostAsync($"users/{user.Id}/claims", ToJsonStringContent(newClaim)))
            {
                if (!postResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not add user {user.Id} to group {clientId}:{groupId}; status code {postResponse.StatusCode}");
                }
            }
        }

        public async Task<User> GetUser(string clientId, string groupId, string userId)
        {
            using (var client = CreateClient())
            using (var getUserResponse = await client.GetAsync($"users/{userId}"))
            {
                if (!getUserResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not get user {userId} for group {clientId}:{groupId}; status code {getUserResponse.StatusCode}");
                }

                using (var getClaimsResponse = await client.GetAsync($"users/{userId}/claims?claimTypes=group"))
                {
                    if (!getClaimsResponse.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException($"Could not get user {userId} for group {clientId}:{groupId}; status code {getClaimsResponse.StatusCode}");
                    }

                    var claimsResponseString = await getUserResponse.Content.ReadAsStringAsync();
                    var claims = (IEnumerable<ClaimDto>)JsonConvert.DeserializeObject(claimsResponseString, typeof(IEnumerable<ClaimDto>));

                    if (!claims.Any(c => c.Type == "group" && c.Value == $"{clientId}:{groupId}"))
                    {
                        throw new InvalidOperationException($"Could not get user {userId} for group {clientId}:{groupId}; user not in group");
                    }
                }

                var responseString = await getUserResponse.Content.ReadAsStringAsync();
                var result = (UserDto)JsonConvert.DeserializeObject(responseString, typeof(UserDto));

                return new User(result.Id, result.Username);
            }
        }

        public async Task<PagingResult<User>> GetUsers(string clientId, string groupId, Paging paging)
        {
            using (var client = CreateClient())
            using (var getResponse = await client.GetAsync($"users?claimType=group&claimValue={clientId}:{groupId}&page={paging.Page}&pageSize={paging.PageSize}"))
            {
                if (!getResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not get users for group {clientId}:{groupId}; status code {getResponse.StatusCode}");
                }

                var responseString = await getResponse.Content.ReadAsStringAsync();
                var result = (PagingResultDto<UserDto>)JsonConvert.DeserializeObject(responseString, typeof(PagingResultDto<UserDto>));
                return new PagingResult<User>(result.Paging.ToDomain(), result.Items.Select(u => new User(u.Id, u.Username)));
            }
        }

        public async Task RemoveUserFromGroup(string clientId, string groupId, string userId)
        {
            using (var client = CreateClient())
            using (var deleteResponse = await client.DeleteAsync($"user/{userId}/claims?claimType=group&value={clientId}:{groupId}"))
            {
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not delete user {userId} from group {clientId}:{groupId}; status code {deleteResponse.StatusCode}");
                }
            }
        }

        private HttpClient CreateClient()
        {
            return new HttpClient { BaseAddress = usersApiUri };
        }

        private StringContent ToJsonStringContent<T>(T obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);

            return new StringContent(serializedObject, Encoding.Unicode, "text/json");
        }
    }
}
