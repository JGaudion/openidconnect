using IdentityServer3.Core.Configuration;
using System.Collections.Generic;
using IdentityServer3.Core.Models;
using System;
using OpenIDConnect.IdentityServer.Services;
using IdentityServer3.Core.Services;
using OpenIDConnect.Core;
using OpenIDConnect.Core.Services;
using IdentityServer3.EntityFramework;
using Owin;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using OpenIDConnect.IdentityServer.Factories;

namespace OpenIDConnect.IdentityServer
{
    internal class IdentityServerOptionsService
    {
        private string adminUsername;

        private string adminPassword;

        private readonly string identityManagerUri;

        private readonly string identityAdminUri;

        private readonly IUserService userService;

        private readonly ExternalIdentityProviderService externalIdentityProviderService;

        public IdentityServerOptionsService(
            string adminUsername,
            string adminPassword,
            string identityManagerUri,
            string identityAdminUri,
            IUserService userService,
            ExternalIdentityProviderService externalIdentityProviderService)
        {
            if (string.IsNullOrWhiteSpace(adminUsername))
            {
                throw new ArgumentNullException("adminUsername");
            }

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new ArgumentNullException("adminPassword");
            }

            if (string.IsNullOrWhiteSpace(identityManagerUri))
            {
                throw new ArgumentNullException("identityManagerUri");
            }

            if (string.IsNullOrWhiteSpace(identityAdminUri))
            {
                throw new ArgumentNullException("identityAdminUri");
            }

            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            if (externalIdentityProviderService == null)
            {
                throw new ArgumentNullException(nameof(externalIdentityProviderService));
            }

            this.adminUsername = adminUsername;
            this.adminPassword = adminPassword;
            this.identityManagerUri = identityManagerUri;
            this.identityAdminUri = identityAdminUri;
            this.userService = userService;
            this.externalIdentityProviderService = externalIdentityProviderService;
        }

        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory();

            var knownClientStore = new KnownClientStore(identityManagerUri, identityAdminUri);

            factory.ClientStore = new Registration<IClientStore>(
                new CompositeClientStore(new IClientStore[] 
                {                    
                    new ClientStore(new ClientConfigurationDbContext("ClientsScopes")),      // TODO: get connection string name from config
                    knownClientStore
                }));

            factory.ScopeStore = new Registration<IScopeStore>(
                new CompositeScopeStore(new IScopeStore[]
                {                    
                    new ScopeStore(new ScopeConfigurationDbContext("ClientsScopes")),        // TODO: get connection string name from config
                    new KnownScopeStore()
                }));

            factory.UserService = new Registration<IUserService>(
                new CompositeUserService(new IUserService[] 
                {                    
                    new ClaimsUserService(new NullClaimsService(), userService),
                    new KnownUserService(this.adminUsername, this.adminPassword)
                }));

            factory.CorsPolicyService = new Registration<ICorsPolicyService>(
                new CompositeCorsPolicyService(new ICorsPolicyService[]
                {                    
                    new ClientConfigurationCorsPolicyService(new ClientConfigurationDbContext("ClientsScopes")),     // TODO: get connection string name from config
                    new InMemoryCorsPolicyService(knownClientStore.GetClients())
                }));                                    

            return new IdentityServerOptions
            {
                SiteName = "IdentityServer v3",
                SigningCertificate = Cert.Load(typeof(IOwinBootstrapper).Assembly, "Cert", "idsrv3test.pfx", "idsrv3test"),
                Endpoints = new EndpointOptions
                {
                    EnableCspReportEndpoint = true
                },
                AuthenticationOptions = new AuthenticationOptions
                {
                    IdentityProviders = externalIdentityProviderService.UseExternalIdentityProviders
                },
                Factory = factory
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