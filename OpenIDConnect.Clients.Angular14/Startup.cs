using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Extensions;

[assembly: OwinStartup(typeof(OpenIDConnect.Clients.Angular14.Startup))]

namespace OpenIDConnect.Clients.Angular14
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
