using OpenIDConnect.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIDConnect.Core.Services
{
    public interface IUserAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateExternalAsync(ExternalIdentity identity, SignInData signInData);
        Task<AuthenticationResult> AuthenticateLocalAsync(string username, string password, SignInData signInData);
        Task<AuthenticationResult> PostAuthenticateAsync(SignInData signInData);
        Task<AuthenticationResult> PreAuthenticateAsync(SignInData signInData);
        Task SignOutAsync(ClaimsPrincipal subject, string clientId);

        Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest request);
        Task<bool> IsActiveAsync(ClaimsPrincipal subject, Client client);
    }
}
