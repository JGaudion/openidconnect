using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System;
using IdentityServer3.Core.Extensions;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using OpenIDConnect.AdLds.Contexts;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Factories;

namespace OpenIDConnect.AdLds.Services
{
    public class AdLdsUserService : UserServiceBase
    {
        private readonly IDirectoryContext directoryContext;

        public AdLdsUserService(IDirectoryContextFactory contextFactory, DirectoryConnectionConfig connectionConfig)
        {
            this.directoryContext = contextFactory.CreateDirectoryContext(connectionConfig);
        }

        public async override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var principal = context.Subject;
            var requestedClaimTypes = context.RequestedClaimTypes;

            var user = await directoryContext.FindUserByNameAsync(principal.Identity.GetName());

            var claims = GetClaimsForResult(user);
        }

        public async override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var username = context.UserName;
            var password = context.Password;

            AuthenticateResult result = null;

            try
            {
                var user = await ValidateLocalCredentials(username, password);
                if (user != null)
                {
                    result = new AuthenticateResult(user.Id, user.Name);
                }
                else
                {
                    result = new AuthenticateResult("Account is not able to log in.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurred while authenticating user {username}: {e}");
                throw;
            }

            context.AuthenticateResult = result;
        }

        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            throw new NotImplementedException("AuthenticateExternalAsync not implemented");
        }

        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject;

            var account = await directoryContext.FindUserByNameAsync(subject.Identity.GetName());

            context.IsActive = account != null;
        }

        private async Task<AdLdsUser> ValidateLocalCredentials(string username, string password)
        {
            return await directoryContext.ValidateCredentialsAsync(username, password);
        }

        private IEnumerable<Claim> GetClaimsForResult(AdLdsUser user)
        {
            yield return new Claim(Constants.ClaimTypes.Subject, user.Id);
            yield return new Claim(Constants.ClaimTypes.PreferredUserName, user.Name);

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                yield return new Claim(Constants.ClaimTypes.Email, user.Email);
            }
        }
    }
}
