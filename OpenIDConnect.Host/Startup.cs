using Microsoft.Owin;
using Owin;
using OpenIDConnect.Host.Interfaces;

[assembly: OwinStartup(typeof(OpenIDConnect.Host.Startup))]

namespace OpenIDConnect.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.Configuration(
                app,
                serverOptionsService: new AspNet.Services.ASPServerOptionsService(),
                adminOptionsService: new AspNet.Services.ASPAdminOptionsService(),
                managerOptionsService: new AspNet.Services.ASPManagerOptionsService()
                //serverOptionsService: new InMemoryServerOptionsService(),
                //adminOptionsService: new InMemoryAdminOptionsService(),
                //managerOptionsService: new InMemoryManagerOptionsService());
               );
        }

        private void Configuration(
            IAppBuilder app,        
            IServerOptionsService serverOptionsService,
            IAdminOptionsService adminOptionsService,
            IManagerOptionsService managerOptionsService
            )
        {
            app.Map("/core", coreApp => {
                var options = serverOptionsService.GetServerOptions();
                coreApp.UseIdentityServer(options);
            });

            var rand = new System.Random();

            app.Map("/admin", adminApp => {
                var options = adminOptionsService.GetAdminOptions();
                adminApp.UseIdentityAdmin(options);
            });

            app.Map("/manage", manageApp =>
            {
                var options = managerOptionsService.GetManagerOptions();
                manageApp.UseIdentityManager(options);
            });
        }
    }
}