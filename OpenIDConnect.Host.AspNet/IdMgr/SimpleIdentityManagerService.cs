using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityManager;
using IdentityManager.AspNetIdentity;
using IdentityManager.Configuration;
using OpenIDConnect.Host.AspNet.EntitiesAndStores;

namespace OpenIDConnect.Host.AspNet.IdMgr
{

    public class SimpleIdentityManagerService : AspNetIdentityManagerService<User, string, Role, string>
    {
        public SimpleIdentityManagerService(UserManager userMgr, RoleManager roleMgr)
          : base(userMgr, roleMgr)
        {
        }
    }

   
}
