using System;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenIDConnect.Users.Domain.Models;

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

            return new ApplicationUser
            {
                UserName = user.Id
            };
        }

        internal User ToDomainModel()
        {
            return new User(this.Id);
        }
    }
}