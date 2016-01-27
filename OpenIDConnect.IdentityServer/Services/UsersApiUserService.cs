using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.Services
{
    public class UsersApiUserService : UserServiceBase
    {
        private readonly string usersApiUri;

        public UsersApiUserService(string usersApiUri)
        {
            this.usersApiUri = usersApiUri;
        }

        public override async Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            {
                if (!(await ExternalUserExistsAsync(client, context.ExternalIdentity.ProviderId)))
                {
                    context.AuthenticateResult = await AddExternalUserAsync(client, context.ExternalIdentity);
                }
            }

            if (context.AuthenticateResult == null)
            {
                context.AuthenticateResult = new AuthenticateResult(
                    context.ExternalIdentity.ProviderId, 
                    GetDisplayName(context.ExternalIdentity.Claims) ?? context.ExternalIdentity.ProviderId, 
                    context.ExternalIdentity.Claims);
            }
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            using (var postResult = await client.PostAsync($"/api/users/{userName}/authenticate", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("password", password) })))
            {

                if (postResult.IsSuccessStatusCode)
                {
                    var claims = await GetClaimsAsync(client, userName, Enumerable.Empty<string>());
                    context.AuthenticateResult = new AuthenticateResult(userName, GetDisplayName(claims) ?? userName, claims);
                }
                else
                {
                    context.AuthenticateResult = new AuthenticateResult($"Could not sign in user {context.UserName}: received status code {postResult.StatusCode}");
                }
            }
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userName = context.Subject.Identity.Name;

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            {
                context.IssuedClaims = await GetClaimsAsync(client, userName, context.RequestedClaimTypes ?? Enumerable.Empty<string>());
            }
        }

        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var userName = context.Subject.Identity.Name;

            using (var client = new HttpClient { BaseAddress = new Uri(this.usersApiUri) })
            {
                context.IsActive = await UserExistsAsync(client, userName);
            }
        }

        public override Task SignOutAsync(SignOutContext context)
        {
            return Task.FromResult(0);
        }

        private async Task<bool> UserExistsAsync(HttpClient client, string userName)
        {
            using (var response = await client.GetAsync($"/api/users/{userName}"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        private Task<bool> ExternalUserExistsAsync(HttpClient client, string userId)
        {
            return Task.FromResult(true);
        }

        private async Task<AuthenticateResult> AddExternalUserAsync(HttpClient client, ExternalIdentity identity)
        {
            using (var response = await client.PostAsync($"/api/users", new StringContent($"{{ id: \"{identity.ProviderId}\", password: \"password\" }}", Encoding.Unicode, "text/json")))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return new AuthenticateResult($"Could not create new external user. Error code: {response.StatusCode}");
                }
            }

            return new AuthenticateResult(identity.ProviderId, GetDisplayName(identity.Claims) ?? identity.ProviderId, identity.Claims);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(HttpClient client, string userName, IEnumerable<string> claimTypes)
        {
            if (claimTypes == null)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            var queryString = claimTypes.Any() ? $"?types={string.Join(",", claimTypes)}" : string.Empty;

            using (var getResult = await client.GetAsync($"/api/users/{userName}/claims{queryString}"))
            {
                var claimsString = await getResult.Content.ReadAsStringAsync();
                var claims = JObject.Parse(claimsString).ToObject<Dictionary<string, string>>();
                return claims.Select(kvp => new Claim(kvp.Key, kvp.Value));
            }
        }

        private string GetDisplayName(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == Core.Constants.ClaimTypes.Name)?.Value;
        }
    }
}
