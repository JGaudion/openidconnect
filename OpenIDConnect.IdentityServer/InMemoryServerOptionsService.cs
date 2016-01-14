using IdentityServer3.Core.Configuration;
using System.Collections.Generic;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.Core.Models;
using System.Linq;
using OpenIDConnect.Core;
using System;
using System.Security.Claims;
using OpenIDConnect.IdentityServer.Services;

namespace OpenIDConnect.IdentityServer
{
    internal class InMemoryServerOptionsService
    {
        private readonly IConfigurationService configurationService;

        private readonly IClientService clientService;

        private readonly IScopeService scopeService;

        public InMemoryServerOptionsService(
            IConfigurationService configurationService,
            IClientService clientService,
            IScopeService scopeService)
        {
            if (configurationService == null)
            {
                throw new ArgumentNullException("configurationService");
            }

            if (clientService == null)
            {
                throw new ArgumentNullException("clientService");
            }

            if (scopeService == null)
            {
                throw new ArgumentNullException("scopeService");
            }

            this.configurationService = configurationService;
            this.clientService = clientService;
            this.scopeService = scopeService;
        }

        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(this.clientService.GetClients())
                .UseInMemoryScopes(this.scopeService.GetScopes())
                .UseInMemoryUsers(this.GetUsers().ToList());            

            return new IdentityServerOptions
            {
                SiteName = "IdentityServer v3",
                SigningCertificate = Cert.Load(),
                Endpoints = new EndpointOptions
                {
                    EnableCspReportEndpoint = true
                },
                Factory = factory
            };
        }

        private IEnumerable<InMemoryUser> GetUsers()
        {
            yield return new InMemoryUser
            {
                Enabled = true,
                Subject = "123",
                Username = "test",
                Password = "test",                
                Claims = new Claim[]
                {
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Name, "test"),
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IdentityAdminManager")
                }
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