
using OpenIdConnect.Host.AdLds.Services;
using OpenIDConnect.Core;
using Owin;

namespace OpenIDConnect.IdentityServer
{
    public class IdentityServerBootstrapper : IOwinBootstrapper
    {
        public void Run(IAppBuilder app)
        {
            var options = new AdLdsServerOptionsService().GetServerOptions();
            app.UseIdentityServer(options);
        }
    }
}
