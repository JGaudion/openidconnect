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

        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            return Task.Run(() =>
            {
                IEnumerable<Scope> scopes = Enumerable.Empty<Scope>();

                foreach (var scopeStore in this.scopeStores)
                {
                    scopes = scopes.Union(scopeStore.FindScopesAsync(scopeNames).Result);
                }

                return scopes;
            });
        }

        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            return Task.Run(() =>
            {
                IEnumerable<Scope> scopes = Enumerable.Empty<Scope>();

                foreach (var scopeStore in this.scopeStores)
                {
                    scopes = scopes.Union(scopeStore.GetScopesAsync(publicOnly).Result);
                }

                return scopes;
            });
        }
    }
}
