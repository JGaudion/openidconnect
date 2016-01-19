using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet.Model
{
    public class RoleStore : RoleStore<Role>
    {
        public RoleStore(AspNetUserStore context) :base (context)
        {

        }
    }
}
