using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.Services
{
    internal class KnownUserService : IUserService 
    {
        private readonly InMemoryUserService inMemoryUserService;        

        public KnownUserService(string adminUsername, string adminPassword)
        {
            if (string.IsNullOrWhiteSpace(adminUsername))
            {
                throw new ArgumentNullException("adminUsername");
            }

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new ArgumentNullException("adminPassword");
            }

            this.inMemoryUserService = 
                new InMemoryUserService(this.GetUsers(adminUsername, adminPassword).ToList());            
        }

        private IEnumerable<InMemoryUser> GetUsers(string adminUsername, string adminPassword)
        {
            yield return new InMemoryUser
            {
                Enabled = true,
                Subject = "123",
                Username = adminUsername,
                Password = adminPassword,
                Claims = new Claim[]
                {
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Name, adminUsername),
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IdentityManagerAdmin"),
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IdentityAdminManager")
                }
            };
        }

        public async Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            await this.inMemoryUserService.PreAuthenticateAsync(context);
        }

        public async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            await this.inMemoryUserService.AuthenticateLocalAsync(context);
        }

        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            context.AuthenticateResult = null;
            return Task.FromResult(0);
        }

        public async Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            await this.inMemoryUserService.PostAuthenticateAsync(context);
        }

        public async Task SignOutAsync(SignOutContext context)
        {
            await this.inMemoryUserService.SignOutAsync(context);
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await this.inMemoryUserService.GetProfileDataAsync(context);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await this.inMemoryUserService.IsActiveAsync(context);
        }
    }
}