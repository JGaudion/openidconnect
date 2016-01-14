using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpenIDConnect.Core.Models
{
    public class Client
    {
        public Client(
            string clientName,
            string clientId,
            IEnumerable<Secret> clientSecrets,
            IEnumerable<string> allowedScopes)
        {
            if (string.IsNullOrWhiteSpace(clientName))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (clientSecrets == null)
            {
                throw new ArgumentNullException(nameof(clientSecrets));
            }

            if (allowedScopes == null)
            {
                throw new ArgumentNullException(nameof(allowedScopes));
            }

            this.ClientName = clientName;
            this.ClientId = clientId;
            this.ClientSecrets = clientSecrets;
            this.AllowedScopes = allowedScopes;
        }

        public string ClientId { get; }
        public string ClientName { get; }
        public string ClientUri { get; set; }

        public IEnumerable<Secret> ClientSecrets { get; }
        public IEnumerable<string> AllowedScopes { get; }

        public IEnumerable<Claim> Claims { get; set; } = Enumerable.Empty<Claim>();

        public Flows Flow { get; set; } = Flows.Implicit;

        public bool LogoutSessionRequired { get; set; } = true;

        public bool Enabled { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public bool AllowAccessToAllCustomGrantTypes { get; set; } = true;
        public bool AllowAccessToAllScopes { get; set; } = true;

        public bool AlwaysSendClientClaims { get; set; } = false;
        public bool PrefixClientClaims { get; set; } = true;

        public int AuthorizationCodeLifetime { get; set; } = 300;
        public int IdentityTokenLifetime { get; set; } = 300;

        public int AccessTokenLifetime { get; set; } = 3600;

        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;
        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;

        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

        public bool RequireConsent { get; set; } = true;
        public bool AllowRememberConsent { get; set; } = true;

        public IEnumerable<string> AllowedCorsOrigins { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> AllowedCustomGrantTypes { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> IdentityProviderRestrictions { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> PostLogoutRedirectUris { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> RedirectUris { get; set; } = Enumerable.Empty<string>();

        public bool AllowClientCredentialsOnly { get; set; }
        public bool IncludeJwtId { get; set; }
        public string LogoUri { get; set; }
        public string LogoutUri { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
    }
}
