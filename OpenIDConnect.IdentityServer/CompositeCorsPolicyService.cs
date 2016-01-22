using IdentityServer3.Core.Services;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer
{
    internal class CompositeCorsPolicyService : ICorsPolicyService
    {
        private readonly IEnumerable<ICorsPolicyService> corsPolicyServices;
        
        public CompositeCorsPolicyService(IEnumerable<ICorsPolicyService> corsPolicyServices)
        {
            if (corsPolicyServices == null)
            {
                throw new ArgumentNullException(nameof(corsPolicyServices));
            }

            this.corsPolicyServices = corsPolicyServices;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            foreach (var corsPolicyService in this.corsPolicyServices)
            {
                var allowedOrigin = await corsPolicyService.IsOriginAllowedAsync(origin);
                if (allowedOrigin)
                {
                    return true;
                }
            }

            return false;
        }
    }
}