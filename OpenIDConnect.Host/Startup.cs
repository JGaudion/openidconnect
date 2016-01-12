using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OpenIDConnect.Host.Startup))]

namespace OpenIDConnect.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.Configuration(
                app, 
                serverOptionsService: new InMemoryServerOptionsService(),
                adminOptionsService: new InMemoryAdminOptionsService());
        }

        private void Configuration(
            IAppBuilder app, 
            IServerOptionsService serverOptionsService,
            IAdminOptionsService adminOptionsService)
        {
            app.Map("/core", coreApp => {
                var options = serverOptionsService.GetServerOptions();
                coreApp.UseIdentityServer(options);
            });

            app.Map("/admin", adminApp => {
                var options = adminOptionsService.GetAdminOptions();
                adminApp.UseIdentityAdmin(options);
            });
        }        
    }
}