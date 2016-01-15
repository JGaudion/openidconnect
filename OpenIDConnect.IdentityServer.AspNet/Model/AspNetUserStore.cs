namespace OpenIDConnect.IdentityServer.AspNet.Model
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AspNetUserStore : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        // Your context has been configured to use a 'AspNetUserStore' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'OpenIDConnect.IdentityServer.AspNet.Model.AspNetUserStore' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AspNetUserStore' 
        // connection string in the application configuration file.
        public AspNetUserStore()
            : base("name=AspNetUserStore")
        {
        }
        public AspNetUserStore(string ConnectionString) :base (ConnectionString)
        {

        }

        
    }

   
}