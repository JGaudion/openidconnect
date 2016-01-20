using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace OpenIDConnect.IdentityServer.Services
{
    public class CompositeScopeStore : IScopeStore
    {
        private readonly IEnumerable<IScopeStore> scopeStores;

        public CompositeScopeStore(IEnumerable<IScopeStore> scopeStores)
        {
            if (scopeStores == null)
            {
                throw new ArgumentNullException("clientStores");
            }

            this.scopeStores = scopeStores;
        }

        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            foreach (var scopeStore in this.scopeStores)
            {
                var scopes = await scopeStore.FindScopesAsync(scopeNames);
                if (scopes != null)
                {
                    return scopes;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            foreach (var scopeStore in this.scopeStores)
            {
                var scopes = await scopeStore.GetScopesAsync(publicOnly);
                if (scopes != null)
                {
                    return scopes;
                }
            }

            return null;
        }
    }
}
