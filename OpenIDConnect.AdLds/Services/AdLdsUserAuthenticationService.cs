using OpenIDConnect.AdLds.Contexts;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.Core.Models;
using OpenIDConnect.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimTypes = OpenIDConnect.Core.Constants.ClaimTypes;

namespace OpenIDConnect.AdLds.Services
{
    public class AdLdsUserAuthenticationService : IUserAuthenticationService
    {
        private readonly IDirectoryContext directoryContext;

        public AdLdsUserAuthenticationService(IDirectoryContextFactory contextFactory, DirectoryConnectionConfig connectionConfig)
        {
            this.directoryContext = contextFactory.CreateDirectoryContext(connectionConfig);
        }

        public Task<AuthenticationResult> AuthenticateExternalAsync(ExternalIdentity identity, SignInData signInData)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> AuthenticateLocalAsync(string username, string password, SignInData signInData)
        {
            try
            {
                var user = await ValidateLocalCredentials(username, password);
                if (user != null)
                {
                    return new AuthenticationResult(user.Id, user.UserName, Enumerable.Empty<Claim>());
                }
                else
                {
                    return new AuthenticationResult("Account is not able to log in.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurred while authenticating user {username}: {e}");
                throw;
            }
        }

        public Task<AuthenticationResult> PostAuthenticateAsync(SignInData signInData, AuthenticationResult authenticationResult)
        {
            return Task.FromResult<AuthenticationResult>(authenticationResult);
        }

        public Task<AuthenticationResult> PreAuthenticateAsync(SignInData signInData)
        {
            return Task.FromResult<AuthenticationResult>(null);
        }

        public Task SignOutAsync(ClaimsPrincipal subject, string clientId)
        {
            return Task.FromResult(0);
        }

        public async Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest request)
        {
            var user = await directoryContext.FindUserByNameAsync(request.Subject.Identity.Name);

            return GetClaimsForResult(user);
        }

        public async Task<bool> IsActiveAsync(ClaimsPrincipal subject, Client client)
        {
            var account = await directoryContext.FindUserByNameAsync(subject.Identity.Name);

            return account != null;
        }

        private async Task<AdLdsUser> ValidateLocalCredentials(string username, string password)
        {
            return await directoryContext.ValidateCredentialsAsync(username, password);
        }

        private IEnumerable<Claim> GetClaimsForResult(AdLdsUser user)
        {
            yield return new Claim(ClaimTypes.Subject, user.Id);
            yield return new Claim(ClaimTypes.PreferredUserName, user.DisplayName);

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                yield return new Claim(ClaimTypes.Email, user.Email);
            }
        }
    }
}
