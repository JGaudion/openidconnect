using IdentityManager;
using IdentityManager.Configuration;
using OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses;

namespace OpenIDConnect.IdentityServer.MembershipReboot
{
    public class MembershipRebootManagerOptionsService
    {
        public IdentityManagerOptions GetManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory();

            factory.IdentityManagerService = new Registration<IIdentityManagerService, CustomIdentityManagerService>();
            factory.Register(new Registration<CustomUserAccountService>());
            factory.Register(new Registration<CustomGroupService>());
            factory.Register(new Registration<CustomUserRepository>());
            factory.Register(new Registration<CustomGroupRepository>());
            factory.Register(new Registration<CustomDatabase>(resolver => new CustomDatabase("MembershipReboot")));
            factory.Register(new Registration<CustomConfig>(CustomConfig.Config));

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