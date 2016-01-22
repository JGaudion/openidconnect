using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot.Ef;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomGroupRepository : DbContextGroupRepository<CustomDatabase, CustomGroup>
    {
        public CustomGroupRepository(CustomDatabase ctx)
            : base(ctx)
        {
        }
    }
}
