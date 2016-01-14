﻿using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace OpenIDConnect.IdentityServer.Services
{
    internal class KnownScopeStore : IScopeStore
    {
        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            var scopes = this.GetScopes();
            return Task.FromResult(scopes.Where(s => scopeNames.Contains(s.Name)));
        }

        // TODO: review publicOnly param
        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var scopes = this.GetScopes();
            return Task.FromResult(scopes);
        }

        private IEnumerable<Scope> GetScopes()
        {
            yield return StandardScopes.OpenId;

            yield return new Scope
            {
                Name = "idmanager",
                DisplayName = "IdentityManager",
                Description = "Authorization for IdentityManager",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Name),
                    new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Role)
                }
            };

            yield return new Scope
            {
                Name = "idadmin",
                DisplayName = "IdentityAdmin",
                Description = "Authorization for IdentityAdmin",
                Type = ScopeType.Identity,
                Claims = new List<ScopeClaim>
                {
                    new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Name),
                    new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Role)
                }                
            };
        }
    }
}