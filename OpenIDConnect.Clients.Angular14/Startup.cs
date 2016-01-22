using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Extensions;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.Jwt;
using System.Collections.Generic;
using IdentityServer3.AccessTokenValidation;
using System;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(OpenIDConnect.Clients.Angular14.Startup))]

namespace OpenIDConnect.Clients.Angular14
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

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

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44333/core",
                RequiredScopes = new[] { "api" },
                NameClaimType = "name",
                RoleClaimType = "role",

                // client credentials for the introspection endpoint
                ClientId = "angualar14",
                ClientSecret = Guid.NewGuid().ToString()
            });

            app.UseWebApi(configuration);

            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
