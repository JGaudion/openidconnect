using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using OpenIDConnect.Host;
using OpenIDConnect.IdentityServer3.AdLds;

namespace OpenIdConnect.Host.AdLds.Services
{
    public class AdLdsServerOptionsService : IServerOptionsService
    {
        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory();

            factory.UserService = new Registration<IUserService, AdLdsUserService>();

            var options = new IdentityServerOptions
            {
                Factory = factory
            };

            return options;
        }
    }
}
