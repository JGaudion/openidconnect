using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Services;
using System.Collections.Generic;

namespace OpenIdConnect.Host.AdLds.Services
{
    public class AdLdsServerOptionsService
    {
        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory()
                   .UseInMemoryClients(this.Clients)
                   .UseInMemoryScopes(this.GetScopes());

            factory.UserService = new Registration<IUserService, AdLdsUserService>();

            factory.Register(new Registration<IDirectoryContextFactory, AdLdsDirectoryContextFactory>());
            factory.Register(new Registration<DirectoryConnectionConfig>(new DirectoryConnectionConfig("localhost", "389", "LDAP://", "CN=ADLDSUsers,DC=ScottLogic,DC=local")));

            var options = new IdentityServerOptions
            {
                Factory = factory
            };

            return options;
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
