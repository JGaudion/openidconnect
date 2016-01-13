using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using OpenIDConnect.Host.AspNet.IdMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.IdSvr
{
    public class Factory
    {
        public static IdentityServerServiceFactory Configure()
        {
            var factory = new IdentityServerServiceFactory();

            //These are temporary because we will need something but would get it from the external parts
            var scopeStore = new IdentityServer3.Core.Services.InMemory.InMemoryScopeStore(TemporaryScopes.Get());
            factory.ScopeStore = new Registration<IScopeStore>(scopeStore);
            var clientStore = new IdentityServer3.Core.Services.InMemory.InMemoryClientStore(TemporaryClients.Get());
            factory.ClientStore = new Registration<IClientStore>(clientStore);

            factory.CorsPolicyService = new Registration<ICorsPolicyService>(new IdentityServer3.Core.Services.Default.DefaultCorsPolicyService { AllowAll = true });

            return factory;
        }

       
    }
}
