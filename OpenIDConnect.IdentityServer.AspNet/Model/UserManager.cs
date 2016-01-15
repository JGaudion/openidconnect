using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet.Model
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store) :base(store)
        {

        }

        
    }
}
