
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

            var options =
                new InMemoryServerOptionsService(configurationService).GetServerOptions();

            app.UseIdentityServer(options);
        }
    }
}