using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Extensions;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Linq;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(OpenIDConnect.Clients.Angular14.Startup))]

namespace OpenIDConnect.Clients.Angular14
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            // Setup camelCase JSON serialization
            var jsonFormatter =
                configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();

            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver
                    = new CamelCasePropertyNamesContractResolver();
            }

            // Set up attribute based routing
            configuration.MapHttpAttributeRoutes();

            app.UseWebApi(configuration);

            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
