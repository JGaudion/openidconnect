using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomGroupService : GroupService<CustomGroup>
    {
        public CustomGroupService(CustomGroupRepository repo, CustomConfig config)
            : base(config.DefaultTenant, repo)
        {

        }
    }
}
