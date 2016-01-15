using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.Services
{
    public class CompositeUserService : IUserService
    {
        private readonly IEnumerable<IUserService> services;

        public CompositeUserService(IEnumerable<IUserService> services)
        {
            this.services = services;
        }

        public async Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            context.AuthenticateResult = null;
            foreach (var service in this.services)
            {
                await service.AuthenticateExternalAsync(context);
                if (context.AuthenticateResult != null && !context.AuthenticateResult.IsError)
                {
                    return;
                }
            }
        }

        public async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            context.AuthenticateResult = null;
            foreach (var service in this.services)
            {
                await service.AuthenticateLocalAsync(context);
                if (context.AuthenticateResult != null && !context.AuthenticateResult.IsError)
                {
                    return;
                }
            }
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            foreach (var service in this.services)
            {
                await service.GetProfileDataAsync(context);
                if (context.IssuedClaims.Any())
                {
                    return;
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;
            foreach (var service in this.services)
            {
                await service.IsActiveAsync(context);
                if (context.IsActive)
                {
                    return;
                }
            }
        }

        public async Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            foreach (var service in this.services)
            {
                await service.PostAuthenticateAsync(context);
            }
        }

        public async Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            foreach (var service in this.services)
            {
                await service.PreAuthenticateAsync(context);
            }
        }

        public async Task SignOutAsync(SignOutContext context)
        {
            foreach (var service in this.services)
            {
                await service.SignOutAsync(context);
            }
        }
    }
}
