using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using IdentityServer3.Core.Models;
using System.Linq;
using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using OpenIDConnect.Host.InMemoryService;

[assembly: OwinStartup(typeof(OpenIDConnect.Host.Startup))]

namespace OpenIDConnect.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/core", coreApp => {
                var options = new IdentityServerOptions
                {
                    Factory = new IdentityServerServiceFactory()
                        .UseInMemoryClients(this.GetClients())
                        .UseInMemoryScopes(this.GetScopes())
                        .UseInMemoryUsers(this.GetUsers().ToList())
                };

                coreApp.UseIdentityServer(options);
            });

            app.Map("/admin", adminApp => {
                var factory = new IdentityAdminServiceFactory
                {
                    IdentityAdminService = new IdentityAdmin.Configuration.Registration<IIdentityAdminService, InMemoryIdentityManagerService>()
                };

                var rand = new System.Random();
                var clients = ClientSeeder.Get(rand.Next(1000, 3000));
                var scopes = ScopeSeeder.Get(rand.Next(15));
                factory.Register(new IdentityAdmin.Configuration.Registration<ICollection<InMemoryScope>>(scopes));
                factory.Register(new IdentityAdmin.Configuration.Registration<ICollection<InMemoryClient>>(clients));
                adminApp.UseIdentityAdmin(new IdentityAdminOptions
                {
                    Factory = factory
                });
            });
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
