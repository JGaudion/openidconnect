
using System.Collections.Generic;
using IdentityServer3.Core.Models;
using System;

namespace OpenIDConnect.IdentityServer.Services
{
    public class DefaultClientService : IClientService
    {
        private readonly string identityManagerUri;

        private readonly string identityAdminUri;

        public DefaultClientService(
            string identityManagerUri,
            string identityAdminUri)
        {
            if (string.IsNullOrWhiteSpace(identityManagerUri))
            {
                throw new ArgumentNullException("identityManagerUri");
            }

            if (string.IsNullOrWhiteSpace(identityAdminUri))
            {
                throw new ArgumentNullException("identityAdminUri");
            }

            this.identityManagerUri = identityManagerUri;
            this.identityAdminUri = identityAdminUri;
        }

        public IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                Enabled = true,
                ClientName = "IdentityManager",
                ClientId = "idmanager_client",
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    this.identityManagerUri
                },
                IdentityProviderRestrictions = new List<string>() { IdentityServer3.Core.Constants.PrimaryAuthenticationType },
                AllowedScopes =
                {
                    IdentityServer3.Core.Constants.StandardScopes.OpenId,
                    "idmanager"
                }
            };

            yield return new Client
            {
                Enabled = true,
                ClientName = "IdentityAdmin",
                ClientId = "idadmin_client",                
                Flow = Flows.Implicit,
                RequireConsent = false,
                RedirectUris = new List<string>
                    {
                        this.identityAdminUri
                    },
                IdentityProviderRestrictions = new List<string>() { IdentityServer3.Core.Constants.PrimaryAuthenticationType },
                AllowedScopes =
                    {
                        IdentityServer3.Core.Constants.StandardScopes.OpenId,
                        "idadmin"
                    }
            };
        }
    }
}