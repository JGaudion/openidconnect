using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using System.Linq;

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
            var scopes = await Task.WhenAll(scopeStores.Select(s => s.FindScopesAsync(scopeNames)).ToArray());
            return scopes.SelectMany(s => s);
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var scopes = await Task.WhenAll(scopeStores.Select(s => s.GetScopesAsync(publicOnly)).ToArray());
            return scopes.SelectMany(s => s);
        }
    }
}
