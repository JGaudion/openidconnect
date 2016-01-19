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
        private readonly IUserManagementService userManagementService;

        public InMemoryManagerOptionsService(IUserManagementService userManagementService)
        {
            this.userManagementService = userManagementService;
        }

        public IdentityManagerOptions GetManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory
            {
                IdentityManagerService = new Registration<IIdentityManagerService>(ctx => new DomainIdentityManagerService(this.userManagementService))
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