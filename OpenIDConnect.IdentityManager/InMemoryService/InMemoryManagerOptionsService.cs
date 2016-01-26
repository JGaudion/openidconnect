using System;
using IdentityManager.Configuration;
using System.Collections.Generic;
using IdentityManager;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityManager.Services;

namespace OpenIDConnect.IdentityManager
{
    internal class InMemoryManagerOptionsService
    {
        private readonly IIdentityManagerService identityManagerService;

        public InMemoryManagerOptionsService(IIdentityManagerService identityManagerService)
        {
            if (identityManagerService == null)
            {
                throw new ArgumentNullException(nameof(identityManagerService));
            }

            this.identityManagerService = identityManagerService;
        }

        public IdentityManagerOptions GetManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory
            {
                IdentityManagerService = new Registration<IIdentityManagerService>(ctx => identityManagerService)
            };

            var rand = new Random();

            var users = UserSeeder.Get(rand.Next(1000, 3000));
            var roles = RoleSeeder.Get(rand.Next(15));
            factory.Register(new Registration<ICollection<InMemoryUser>>(users));
            factory.Register(new Registration<ICollection<InMemoryRole>>(roles));

            return new IdentityManagerOptions
            {
                Factory = factory,
                SecurityConfiguration = new HostSecurityConfiguration
                {
                    HostAuthenticationType = "Cookies",
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    AdminRoleName = "IdentityManagerAdmin"
                }
            };
        }
    }
}