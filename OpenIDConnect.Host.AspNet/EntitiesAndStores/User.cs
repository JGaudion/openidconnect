using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.EntitiesAndStores
{
    /// <summary>
    /// Manual implemenation of the IdentityUser class. It is recommended by Brock Allen to manually implement 
    /// all of the entities
    /// </summary>
    public class User :IdentityUser
    {
        //Optional extra fields - will appear as columns in my database
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
