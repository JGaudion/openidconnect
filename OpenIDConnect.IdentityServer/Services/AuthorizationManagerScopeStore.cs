using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace OpenIDConnect.IdentityServer.Services
{
    internal class AuthorizationManagerScopeStore : FixedScopeStore
    {
        protected override IEnumerable<Scope> GetScopes()
        {
            yield return new Scope
            {
                Name = "permissions-resource",
                DisplayName = "Permissions",
                Description = "Permissions from the authorization manager",
                Type = ScopeType.Resource,
                ShowInDiscoveryDocument = true,
                Enabled = true,
                Required = true,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("Permission")
                }
            };

            yield return new Scope
            {
                Name = "permissions-identity",
                DisplayName = "Permissions",
                Description = "Permissions from the authorization manager",
                Type = ScopeType.Identity,
                ShowInDiscoveryDocument = true,
                Enabled = true,
                Required = true,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim("Permission")
                }
            };
        }
    }
}
