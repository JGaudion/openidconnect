using BrockAllen.MembershipReboot;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomGroup : RelationalGroup
    {
        public virtual string Description { get; set; }
    }
}