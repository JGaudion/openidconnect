using System;
using IdentityManager.Configuration;
using System.Collections.Generic;
using IdentityManager;

namespace OpenIDConnect.Host.InMemoryService
{
    internal class InMemoryManagerOptionsService : IManagerOptionsService
    {
        public IdentityManagerOptions GetManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory
            {
                IdentityManagerService = new Registration<IIdentityManagerService, InMemoryIdentityManagerService>()
            };

            var rand = new Random();

            var users = UserSeeder.Get(rand.Next(1000, 3000));
            var roles = RoleSeeder.Get(rand.Next(15));
            factory.Register(new Registration<ICollection<InMemoryUser>>(users));
            factory.Register(new Registration<ICollection<InMemoryRole>>(roles));

            return new IdentityManagerOptions
            {
                Factory = factory
            };
        }
    }
}