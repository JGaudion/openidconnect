using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    /// <summary>
    /// My implementation of the Role Manager
    /// </summary>
    public class RoleManager :RoleManager<Role>
    {
        public RoleManager(RoleStore store) : base(store)
        {

        }
    }
}
