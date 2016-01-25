using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;
using OpenIDConnect.Users.Domain;
using OpenIDConnect.Users.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    public class AspNetIdentityUsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AspNetIdentityUsersRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUser(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            return user?.ToDomainModel();
        }

        public async Task AddUser(User user)
        {
            var applicationUser = ApplicationUser.FromUser(user);
            var result = await this.userManager.CreateAsync(applicationUser);
            if (!result.Succeeded)
            {
                // TODO: change to identity create exception
                throw new Exception("There was an error adding the user");
            }
        }
    }
}