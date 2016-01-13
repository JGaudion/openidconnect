using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
           : base(store)
        {
            this.ClaimsIdentityFactory = new ClaimsFactory();
        }
    }
}
