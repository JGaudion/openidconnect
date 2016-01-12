using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using OpenIDConnect.Host.InMemoryService;
using System.Collections.Generic;

namespace OpenIDConnect.Host
{
    internal class InMemoryAdminOptionsService : IAdminOptionsService
    {
        public IdentityAdminOptions GetAdminOptions()
        {
            var factory = new IdentityAdminServiceFactory
            {
                IdentityAdminService = new Registration<IIdentityAdminService, InMemoryIdentityAdminService>()
            };

            var rand = new System.Random();
            var clients = ClientSeeder.Get(rand.Next(1000, 3000));
            var scopes = ScopeSeeder.Get(rand.Next(15));

            factory.Register(new Registration<ICollection<InMemoryScope>>(scopes));
            factory.Register(new Registration<ICollection<InMemoryClient>>(clients));

            return new IdentityAdminOptions
            {
                Factory = factory
            };
        }
    }
}