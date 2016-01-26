using System;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.IdentityServer.Services
{
    public interface IClaimsService
    {
        IEnumerable<Claim> GetUserClaimsForClient(string clientId, string userId);
    }

    public class NullClaimsService : IClaimsService
    {
        public IEnumerable<Claim> GetUserClaimsForClient(string clientId, string userId)
        {
            yield return new Claim("Permission", "TestPermission");
        }
    }

    public class ClaimsUserService : IUserService
    {
        private readonly IClaimsService claimsService;

        private readonly IUserService innerUserService;

        public ClaimsUserService(IClaimsService claimsService, IUserService innerUserService)
        {
            if (claimsService == null)
            {
                throw new ArgumentNullException(nameof(claimsService));
            }

            if (innerUserService == null)
            {
                throw new ArgumentNullException(nameof(innerUserService));
            }

            this.claimsService = claimsService;
            this.innerUserService = innerUserService;
        }

        public async Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            await this.innerUserService.AuthenticateExternalAsync(context);
        }

        public async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            await this.innerUserService.AuthenticateLocalAsync(context);
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await this.innerUserService.GetProfileDataAsync(context);
            if (context.IssuedClaims == null)
            {
                return;
            }

            var userClientClaims = this.claimsService.GetUserClaimsForClient(
                context.Client?.ClientId,
                context.Subject?.Identity?.Name);

            var finalClaims = context.IssuedClaims.Union(userClientClaims);
            context.IssuedClaims = finalClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            await this.innerUserService.IsActiveAsync(context);
        }

        public async Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            await this.innerUserService.PostAuthenticateAsync(context);
        }

        public async Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            await this.innerUserService.PreAuthenticateAsync(context);
        }

        public async Task SignOutAsync(SignOutContext context)
        {
            await this.innerUserService.SignOutAsync(context);
        }
    }
}