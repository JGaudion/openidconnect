using AutoMapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using OpenIDConnect.Core.Models;
using OpenIDConnect.Core.Services;
using System;
using System.Threading.Tasks;
using DomainClient = OpenIDConnect.Core.Models.Client;
using DomainExternalIdentity = OpenIDConnect.Core.Models.ExternalIdentity;
using IdentityServerClient = IdentityServer3.Core.Models.Client;
using IdentityServerExternalIdentity = IdentityServer3.Core.Models.ExternalIdentity;

namespace OpenIDConnect.IdentityServer.Services
{
    public class DomainUserService : IUserService
    {
        private readonly IUserAuthenticationService domainService;

        public DomainUserService(IUserAuthenticationService domainService)
        {
            if (domainService == null)
            {
                throw new ArgumentNullException(nameof(domainService));
            }

            this.domainService = domainService;
        }

        public async Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            var externalIdentity = Mapper.Map<IdentityServerExternalIdentity, DomainExternalIdentity>(context.ExternalIdentity);
            var signInData = Mapper.Map<SignInMessage, SignInData>(context.SignInMessage);
            var result = await domainService.AuthenticateExternalAsync(externalIdentity, signInData);

            context.AuthenticateResult = Mapper.Map<AuthenticationResult, AuthenticateResult>(result);
        }

        public async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var signInData = Mapper.Map<SignInMessage, SignInData>(context.SignInMessage);
            var result = await domainService.AuthenticateLocalAsync(context.UserName, context.Password, signInData);

            context.AuthenticateResult = Mapper.Map<AuthenticationResult, AuthenticateResult>(result);
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var profileRequest = Mapper.Map<ProfileDataRequestContext, ProfileDataRequest>(context);
            var result = await domainService.GetProfileDataAsync(profileRequest);

            context.IssuedClaims = result;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var client = Mapper.Map<IdentityServerClient, DomainClient>(context.Client);
            var result = await domainService.IsActiveAsync(context.Subject, client);

            context.IsActive = result;
        }

        public async Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            var signInData = Mapper.Map<SignInMessage, SignInData>(context.SignInMessage);
            var authenticationResult = Mapper.Map<AuthenticateResult, AuthenticationResult>(context.AuthenticateResult);
            var result = await domainService.PostAuthenticateAsync(signInData, authenticationResult);

            context.AuthenticateResult = Mapper.Map<AuthenticationResult, AuthenticateResult>(result);
        }

        public async Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            var signInData = Mapper.Map<SignInMessage, SignInData>(context.SignInMessage);
            var result = await domainService.PreAuthenticateAsync(signInData);

            context.AuthenticateResult = Mapper.Map<AuthenticationResult, AuthenticateResult>(result);
        }

        public async Task SignOutAsync(SignOutContext context)
        {
            await domainService.SignOutAsync(context.Subject, context.ClientId);
        }
    }
}
