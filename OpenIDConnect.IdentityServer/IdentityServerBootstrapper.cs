using AutoMapper;
using OpenIDConnect.Core;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityServer.Configuration;
using OpenIDConnect.IdentityServer.Factories;
using Owin;
using System;

namespace OpenIDConnect.IdentityServer
{
    public class IdentityServerBootstrapper : IOwinBootstrapper
    {
        private readonly IConfigurationService configService;
        private readonly ExternalIdentityProviderService externalIdentityProviderService;
        private readonly IUserAuthenticationService userAuthenticationService;

        public IdentityServerBootstrapper(IUserAuthenticationService userAuthenticationService,
            ExternalIdentityProviderService externalIdentityProviderService,
            IConfigurationService configService)
        {
            if (userAuthenticationService == null)
            {
                throw new ArgumentNullException(nameof(userAuthenticationService));
            }

            if (externalIdentityProviderService == null)
            {
                throw new ArgumentNullException(nameof(externalIdentityProviderService));
            }

            if (configService == null)
            {
                throw new ArgumentNullException(nameof(configService));
            }

            this.userAuthenticationService = userAuthenticationService;
            this.externalIdentityProviderService = externalIdentityProviderService;
            this.configService = configService;
        }

        public void Run(IAppBuilder app)
        {
            MappingConfiguration.Configure();

            Mapper.AssertConfigurationIsValid();

            var adminUsername = this.configService.GetSetting<string>("AdminUsername", null);
            var adminPassword = this.configService.GetSetting<string>("AdminPassword", null);
            var identityManagerUri = this.configService.GetSetting<string>("IdentityManagerUri", null);
            var identityAdminUri = this.configService.GetSetting<string>("IdentityAdminUri", null);

            var options =
                new IdentityServerOptionsService(
                    adminUsername, 
                    adminPassword, 
                    identityManagerUri, 
                    identityAdminUri,
                    userAuthenticationService,
                    externalIdentityProviderService).GetServerOptions();

            app.UseIdentityServer(options);
        }
    }
}