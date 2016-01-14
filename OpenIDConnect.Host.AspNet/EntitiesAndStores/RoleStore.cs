using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    /// <summary>
    /// This is my implementation of the RoleStore. It is needed for the Application Role Manager
    /// </summary>
    public class RoleStore : RoleStore<Role>
    {
        /// <summary>
        /// The base class needs to know the context (that communicates with the data). 
        /// The Context is an implementation of IdentityDbContext<MyUserImplementation> 
        /// </summary>
        /// <param name="ctx"></param>
        public RoleStore(Context ctx) :base (ctx)
        {

        }
    }
}
