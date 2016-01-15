
using AutoMapper;
using OpenIDConnect.Core;
using OpenIDConnect.IdentityServer.Configuration;
using Owin;

namespace OpenIDConnect.IdentityServer
{
    public class IdentityServerBootstrapper : IOwinBootstrapper
    {
        public void Run(IAppBuilder app)
        {
            MappingConfiguration.Configure();

            Mapper.AssertConfigurationIsValid();

            var configurationService = new ApplicationSettingsConfigurationService();

            var adminUsername = configurationService.GetSetting<string>("AdminUsername", null);
            var adminPassword = configurationService.GetSetting<string>("AdminPassword", null);
            var identityManagerUri = configurationService.GetSetting<string>("IdentityManagerUri", null);
            var identityAdminUri = configurationService.GetSetting<string>("IdentityAdminUri", null);

            var options =
                new InMemoryServerOptionsService(
                    adminUsername, 
                    adminPassword, 
                    identityManagerUri, 
                    identityAdminUri).GetServerOptions();

            app.UseIdentityServer(options);
        }
    }
}