using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    public class UserStore :UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(Context context) :base(context)
        {

        }
    }
}
