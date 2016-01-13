using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    public class User :IdentityUser
    {
        //Optional extra fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
