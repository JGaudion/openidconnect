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

        public InMemoryServerOptionsService(
            IConfigurationService configurationService)
        {
            if (configurationService == null)
            {
                throw new ArgumentNullException("configurationService");
            }

            this.configurationService = configurationService;
        }

        public IdentityServerOptions GetServerOptions()
        {
            var identityManagerUri = configurationService.GetSetting<string>("IdentityManagerUri", null);
            var identityAdminUri = configurationService.GetSetting<string>("IdentityAdminUri", null);

            var factory = new IdentityServerServiceFactory()
                .UseInMemoryUsers(this.GetUsers().ToList());
            
            factory.ClientStore = new Registration<IdentityServer3.Core.Services.IClientStore>(new KnownClientStore(identityManagerUri, identityAdminUri));
            factory.ScopeStore = new Registration<IdentityServer3.Core.Services.IScopeStore>(new KnownScopeStore());

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
                Username = "admin",
                Password = "admin",                
                Claims = new Claim[]
                {
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Name, "admin"),
                    new Claim(IdentityServer3.Core.Constants.ClaimTypes.Role, "IdentityManagerAdmin"),
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