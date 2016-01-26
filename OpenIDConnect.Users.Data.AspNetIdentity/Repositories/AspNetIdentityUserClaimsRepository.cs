using OpenIDConnect.Users.Domain.Repositories;
using System;
using System.Threading.Tasks;
using OpenIDConnect.Users.Domain.Models;
using Microsoft.AspNet.Identity;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;
using OpenIDConnect.Users.Domain;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    public class AspNetIdentityUserClaimsRepository : IUserClaimsRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        public AspNetIdentityUserClaimsRepository(            
            UserManager<ApplicationUser> userManager)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user specified");
            }

            var userClaims = await this.userManager.GetClaimsAsync(user);
            return userClaims.Select(c => new Claim(c.Type, c.Value)).ToList();
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsOfType(string username, string claimType)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            var userClaims = await this.GetUserClaims(username);

            return userClaims
                .Where(c => string.Compare(c.Type, claimType, StringComparison.OrdinalIgnoreCase) == 0)
                .ToList();
        }

        public async Task AddClaimsToUser(string username, IEnumerable<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user specified");
            }

            var identityResult = await this.userManager.AddClaimsAsync(
                user,
                claims.Select(c => new System.Security.Claims.Claim(c.Type, c.Value)));

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException("There was an error adding claims to the user");
            }
        }
    }
}