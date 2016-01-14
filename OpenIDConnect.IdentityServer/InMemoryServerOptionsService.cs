using IdentityServer3.Core.Configuration;
using System.Collections.Generic;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.Core.Models;
using System.Linq;
using OpenIDConnect.Core;
using System;
using static IdentityServer3.Core.Constants;
using System.Security.Claims;

namespace OpenIDConnect.IdentityServer
{
    internal class InMemoryServerOptionsService
    {
        private readonly IConfigurationService configurationService;

        public InMemoryServerOptionsService(IConfigurationService configurationService)
        {
            if (configurationService == null)
            {
                throw new ArgumentNullException("configurationService");
            }

            this.configurationService = configurationService;
        }

        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryClients(this.Clients)
                .UseInMemoryScopes(this.GetScopes())
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

        private IEnumerable<Scope> GetScopes()
        {
            yield return IdentityServer3.Core.Models.StandardScopes.OpenId;
            yield return new Scope
            {
                Name = "idadmin",
                DisplayName = "IdentityAdmin",
                Description = "Authorization for IdentityAdmin",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>{
                        new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Name),
                        new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Role)
                    }
            };
        }

        private IEnumerable<Client> Clients
        {
            get
            {
                var identityAdminUri = this.configurationService.GetSetting<string>("IdentityAdminUri", null);
                if (string.IsNullOrWhiteSpace(identityAdminUri))
                {
                    throw new InvalidOperationException("No identity admin uri specified");
                }

                yield return new Client
                {
                    ClientName = "IdentityAdmin",
                    ClientId = "idadmin_client",
                    Enabled = true,                    
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        identityAdminUri
                    },
                    IdentityProviderRestrictions = new List<string>() { IdentityServer3.Core.Constants.PrimaryAuthenticationType },
                    AllowedScopes =
                    {
                        IdentityServer3.Core.Constants.StandardScopes.OpenId,
                        "idadmin"
                    }
                };

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