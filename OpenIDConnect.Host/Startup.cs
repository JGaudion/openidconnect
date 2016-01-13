using Microsoft.Owin;
using Owin;
using OpenIDConnect.IdentityServer;
using OpenIDConnect.IdentityAdmin;
using OpenIDConnect.IdentityManager;

[assembly: OwinStartup(typeof(OpenIDConnect.Host.Startup))]

namespace OpenIDConnect.Host
{
    public class Startup
    {        
        public void Configuration(IAppBuilder app)
        {
            app.Map("/core", coreApp => {
                new IdentityServerBootstrapper().Run(coreApp);
            });

            app.Map("/admin", adminApp => {
                new IdentityAdminBootstrapper().Run(adminApp);                
            });

            app.Map("/manage", manageApp =>
            {
                new IdentityManagerBootstrapper().Run(manageApp);                
            });
        }
    }
}