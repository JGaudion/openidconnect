using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenIDConnect.Authorization.Domain.Models;
using OpenIDConnect.Authorization.Domain.Repositories;
using OpenIDConnect.Core.Domain.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using OpenIDConnect.Authorization.Data.UsersApi.Models;
using System.Linq;

namespace OpenIDConnect.Authorization.Data.UsersApi.Repositories
{
    public class UsersApiUsersRepository : IUsersRepository
    {
        private readonly Uri usersApiUri;
        private readonly IGroupClaimsRepository groupClaimsRepository;

        public UsersApiUsersRepository(string usersApiUri, IGroupClaimsRepository groupClaimsRepository)
        {
            if (string.IsNullOrWhiteSpace(usersApiUri))
            {
                throw new ArgumentNullException(nameof(usersApiUri));
            }

            if (groupClaimsRepository == null)
            {
                throw new ArgumentNullException(nameof(groupClaimsRepository));
            }

            this.usersApiUri = new Uri(new Uri(usersApiUri), "api/");

            this.groupClaimsRepository = groupClaimsRepository;
        }

        public async Task<PagingResult<User>> GetUsers(Paging paging)
        {
            using (var client = CreateClient())
            using (var getResponse = await client.GetAsync($"users?pageSize={paging.PageSize}&page={paging.Page}"))
            {
                if (!getResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not get users; status code {getResponse.StatusCode}");
                }

                var responseString = await getResponse.Content.ReadAsStringAsync();
                var result = (PagingResultDto<UserDto>)JsonConvert.DeserializeObject(responseString, typeof(PagingResultDto<UserDto>));
                return new PagingResult<User>(result.Paging.ToDomain(), result.Items.Select(u => new User(u.Id, u.Username)));
            }
        }

        public async Task<IEnumerable<ClientGroup>> GetUserGroups(string userId)
        {
            using (var client = CreateClient())
            using (var getResponse = await client.GetAsync($"users/{userId}/claims?claimType=group"))
            {
                if (!getResponse.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Could not get users; status code {getResponse.StatusCode}");
                }

                var responseString = await getResponse.Content.ReadAsStringAsync();
                var groupClaims = (IEnumerable<ClaimDto>)JsonConvert.DeserializeObject(responseString, typeof(IEnumerable<ClaimDto>));
                return groupClaims.Where(c => !string.IsNullOrEmpty(c.Value)) // Only those claims with a value
                    .Select(c => c.Value.Split(':')) // Split to client:group
                    .Where(cg => cg.Length == 2) // Only those that have been formatted correctly
                    .Select(cg => new ClientGroup(cg[0], cg[1])); // Turn it into a group
            }
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string userId)
        {
            var userGroups = await this.GetUserGroups(userId);

            var claims = (await Task.WhenAll(userGroups.Select(g => this.groupClaimsRepository.GetClaims(g.ClientId, g.Id)).ToArray())).SelectMany(c => c);

            return claims.Select(c => new Claim(c.Type, c.Value));
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
