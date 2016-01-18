using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityServer.Factories;
using OpenIDConnect.IdentityServer.Services;
using System;
using System.Collections.Generic;

namespace OpenIDConnect.IdentityServer
{
    internal class IdentityServerOptionsService
    {
        private string adminUsername;

        private string adminPassword;

        private readonly string identityManagerUri;

        private readonly string identityAdminUri;
        private readonly IUserAuthenticationService userAuthenticationService;

        public IdentityServerOptionsService(
            string adminUsername,
            string adminPassword,
            string identityManagerUri, 
            string identityAdminUri,
            IUserAuthenticationService userAuthenticationService)
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

            if (userAuthenticationService == null)
            {
                throw new ArgumentNullException(nameof(userAuthenticationService));
            }

            this.adminUsername = adminUsername;
            this.adminPassword = adminPassword;
            this.identityManagerUri = identityManagerUri;
            this.identityAdminUri = identityAdminUri;

            this.userAuthenticationService = userAuthenticationService;
        }

        public IdentityServerOptions GetServerOptions()
        {
            var factory = new IdentityServerServiceFactory();
                        
            factory.ClientStore = new Registration<IClientStore>(
                new CompositeClientStore(new IClientStore[] {
                    new KnownClientStore(identityManagerUri, identityAdminUri),
                    new IdentityAdminClientStore(this.identityAdminUri)                    
                }));

            factory.ScopeStore = new Registration<IScopeStore>(new KnownScopeStore());
            factory.UserService = new Registration<IUserService>(new CompositeUserService(new IUserService[] {
                new KnownUserService(this.adminUsername, this.adminPassword),
                new DomainUserService(userAuthenticationService)
            }));

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