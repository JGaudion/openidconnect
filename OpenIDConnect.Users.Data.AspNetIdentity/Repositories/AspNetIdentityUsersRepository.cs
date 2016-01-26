using Microsoft.AspNet.Identity;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;
using OpenIDConnect.Users.Domain;
using OpenIDConnect.Users.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OpenIDConnect.Core.Domain.Models;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    public class AspNetIdentityUsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AspNetIdentityUsersRepository(UserManager<ApplicationUser> userManager)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            this.userManager = userManager;
        }

        public async Task<User> GetUserByName(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
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

        public async Task UpdateUser(User user)
        {
            var applicationUser = ApplicationUser.FromUser(user);
            var result = await this.userManager.UpdateAsync(applicationUser);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("There was an error updating the user");
            }
        }

        public async Task DeleteUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid username specified");
            }

            var result = await this.userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("There was an error deleting the user");
            }
        }

        public async Task<bool> Authenticate(string userId, string password)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await this.GetUserByName(userId);
            if (user == null)
            {
                return false;
            }

            return await this.userManager.CheckPasswordAsync(
                ApplicationUser.FromUser(user), 
                password);
        }

        public Task<PagingResult<User>> QueryUsers(string username, Paging paging)
        {
            return Task.Run(() =>
                {
                    var users = this.userManager.Users
                                    .Where(u => username == null || u.Id.Contains(username))
                                    .Select(u => new User(u.Id, u.UserName, u.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue))))
                                    .Skip(paging.Page * paging.PageSize)
                                    .Take(paging.PageSize)
                                    .ToList();

                    var total = this.userManager.Users.Count();

                    return new PagingResult<User>(paging.Page, paging.PageSize, users.Count, total, users);
                });
        }
    }
}