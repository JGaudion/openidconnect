using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    public class RoleStore : RoleStore<Role>
    {
        public RoleStore(Context ctx) :base (ctx)
        {

        }
    }
}
