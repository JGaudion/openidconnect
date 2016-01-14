using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace OpenIDConnect.IdentityServer.Services
{
    internal class DefaultScopeService : IScopeService
    {
        public IEnumerable<Scope> GetScopes()
        {
            yield return StandardScopes.OpenId;
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
    }
}
