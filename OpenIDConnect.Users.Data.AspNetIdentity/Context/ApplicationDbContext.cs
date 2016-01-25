using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}