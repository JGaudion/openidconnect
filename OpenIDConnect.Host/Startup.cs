using Microsoft.Owin;
using Owin;
using IdentityAdmin.Logging;
using Serilog;
using Autofac;
using OpenIDConnect.IdentityManager;
using OpenIDConnect.IdentityAdmin;
using OpenIDConnect.IdentityServer;

[assembly: OwinStartup(typeof(OpenIDConnect.Host.Startup))]

namespace OpenIDConnect.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            var builder = new ContainerBuilder();

            Modules.Register(builder);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                app.Map("/core", coreApp =>
                {
                    scope.Resolve<IdentityServerBootstrapper>().Run(coreApp);
                });

                app.Map("/admin", adminApp =>
                {
                    scope.Resolve<IdentityAdminBootstrapper>().Run(adminApp);
                });

                app.Map("/manage", manageApp =>
                {
                    scope.Resolve<IdentityManagerBootstrapper>().Run(manageApp);
                });
            }
        }
    }
}