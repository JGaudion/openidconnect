using System.Collections.Generic;
using System.Linq;

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

using Microsoft.Owin;

using Owin;

[assembly: OwinStartup(typeof(OpenIdConnectSPAExample.IdentityServer.Startup1))]

namespace OpenIdConnectSPAExample.IdentityServer
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                        .UseInMemoryClients(this.GetClients())
                        .UseInMemoryScopes(this.GetScopes())
                        .UseInMemoryUsers(this.GetUsers().ToList())
            };

            app.UseIdentityServer(options);
        }

        private IEnumerable<InMemoryUser> GetUsers()
        {
            yield return new InMemoryUser
            {
                Enabled = true,
                Username = "test",
                Password = "test",
                Subject = "123"
            };
        }

        private IEnumerable<Scope> GetScopes()
        {
            yield return new Scope
            {
                Name = "api1"                
            };
        }

        private IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientName = "Angular",
                ClientId = "angular",
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,
                Flow = Flows.Implicit,
                ClientSecrets = new List<Secret> { new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256()) },
                AllowedScopes = new List<string> { "api" }
            };
        }
    }
}