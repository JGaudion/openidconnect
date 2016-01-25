using System;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenIDConnect.Users.Domain.Models;
using System.Linq;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        internal static ApplicationUser FromUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var applicationUser = new ApplicationUser
            {
                UserName = user.Id                
            };

            foreach (var claim in user.Claims)
            {
                applicationUser.Claims.Add(new IdentityUserClaim<string>()
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }

            return applicationUser;    
        }

        internal User ToDomainModel()
        {
            return new User(
                this.Id, 
                this.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));
        }
    }
}