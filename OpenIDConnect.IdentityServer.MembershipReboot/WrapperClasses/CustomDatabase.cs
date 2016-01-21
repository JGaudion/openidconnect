using BrockAllen.MembershipReboot.Ef;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomDatabase : MembershipRebootDbContext<CustomUser, CustomGroup>
    {
        public CustomDatabase(string name)
            :base(name)
        {
        }
    }
}