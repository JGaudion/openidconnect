using IdentityServer3.Core.Configuration;
using System.Collections.Generic;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.Core.Models;
using System.Linq;

namespace OpenIDConnect.IdentityServer
{
    internal class InMemoryServerOptionsService
    {
        public IdentityServerOptions GetServerOptions()
        {
            return new IdentityServerOptions
            {                
                Factory = new IdentityServerServiceFactory()
                   .UseInMemoryClients(this.Clients)
                   .UseInMemoryScopes(this.GetScopes())
                   .UseInMemoryUsers(this.GetUsers().ToList())
            };
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

        private IEnumerable<Client> Clients
        {
            get
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
}