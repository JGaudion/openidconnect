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
    /// <summary>
    /// This is my implementation of the Identity Manager service.  It needs to know which class I am using for the User and the Role
    /// along with the type of primary key
    /// </summary>
    public class SimpleIdentityManagerService : AspNetIdentityManagerService<User, string, Role, string>
    {
        /// <summary>
        /// The base class needs to know what the User Manager and Role Manager are (ie my implementation of the Microsoft.AspNet.Identity.RoleManager)
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="roleMgr"></param>
        public SimpleIdentityManagerService(UserManager userMgr, RoleManager roleMgr)
          : base(userMgr, roleMgr)
        {
        }
    }

   
}
