
using System.Web.Http;
using Microsoft.Owin.Extensions;
using Owin;


namespace OpenIDConnect.Clients.AngularMaterial
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            //app.UseWebApi(configuration);

            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}