
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

            var options =
                new InMemoryServerOptionsService(configurationService).GetServerOptions();

            app.UseIdentityServer(options);
        }
    }
}