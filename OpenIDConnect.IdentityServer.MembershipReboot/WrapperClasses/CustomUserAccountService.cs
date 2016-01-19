using BrockAllen.MembershipReboot;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomUserAccountService : UserAccountService<CustomUser>
    {
        public CustomUserAccountService(CustomConfig config, CustomUserRepository repo)
            : base(config, repo)
        {
        }
    }
}