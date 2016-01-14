using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityAdmin.Configuration;
using IdentityManager.Configuration;
using OpenIDConnect.Host.AspNet.IdSvr;


namespace OpenIDConnect.Host.AspNet.Services
{
    public class ASPAdminOptionsService
    {
        public IdentityAdminOptions GetAdminOptions()
        {
            var factory = new IdentityAdminServiceFactory();
            factory.IdentityAdminService = new IdentityAdmin.Configuration.Registration<IdentityAdmin.Core.IIdentityAdminService, IdentityAdminServiceTemp>();
                       
            var clients = TemporaryClients.Get();
            var scopes = TemporaryScopes.Get();

            factory.Register(new IdentityAdmin.Configuration.Registration<IEnumerable<IdentityServer3.Core.Models.Scope>>(scopes));
            factory.Register(new IdentityAdmin.Configuration.Registration<List<IdentityServer3.Core.Models.Client>>(clients));

            return new IdentityAdminOptions
            {
                Factory = factory
            };
        }
    }
}
