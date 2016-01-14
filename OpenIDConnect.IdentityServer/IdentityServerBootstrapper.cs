
using OpenIDConnect.Core;
using OpenIDConnect.IdentityServer.Services;
using Owin;

namespace OpenIDConnect.IdentityServer
{
    public class IdentityServerBootstrapper : IOwinBootstrapper
    {
        public void Run(IAppBuilder app)
        {
            var configurationService = new ApplicationSettingsConfigurationService();

            var identityManagerUri = configurationService.GetSetting<string>("IdentityManagerUri", null);
            var identityAdminUri = configurationService.GetSetting<string>("IdentityAdminUri", null);

            var options =
                new InMemoryServerOptionsService(
                    configurationService, 
                    new DefaultClientService(identityManagerUri, identityAdminUri),
                    new DefaultScopeService()).GetServerOptions();

            app.UseIdentityServer(options);
        }
    }
}