using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Extensions;
using Newtonsoft.Json.Serialization;
using Owin;


namespace OpenIDConnect.Clients.AngularMaterial
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44333/core",
                RequiredScopes = new[] { "api" },
                NameClaimType = "name",
                RoleClaimType = "role",

                // client credentials for the introspection endpoint
                ClientId = "angularMaterial",
                ClientSecret = Guid.NewGuid().ToString()
            });

            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            var jsonFormatter = configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();

            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            app.UseResourceAuthorization(new AuthorizationManager());

            app.UseWebApi(configuration);

            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}