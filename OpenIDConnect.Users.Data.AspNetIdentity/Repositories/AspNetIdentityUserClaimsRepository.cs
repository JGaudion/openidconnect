using OpenIDConnect.Users.Domain.Repositories;
using System;
using System.Threading.Tasks;
using OpenIDConnect.Users.Domain.Models;
using Microsoft.AspNet.Identity;
using OpenIDConnect.Users.Data.AspNetIdentity.Models;
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

        public async Task<IEnumerable<Claim>> GetUserClaimsOfTypes(string username, IEnumerable<string> claimTypes)
        {
            var enumerable = claimTypes as string[] ?? claimTypes.ToArray();
            if (claimTypes == null || !enumerable.Any())
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            var userClaims = await this.GetUserClaims(username);

            return userClaims
                .Where(c => enumerable.Any(ct => string.Compare(c.Type, ct, StringComparison.OrdinalIgnoreCase) == 0))
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
            
            var userClaims = await this.userManager.GetClaimsAsync(user);
            
            var identityResult = await this.userManager.AddClaimsAsync(
                user,
                claims.Select(c => new System.Security.Claims.Claim(c.Type, c.Value)));

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException("There was an error adding claims to the user");
            }
        }

        public async Task UpdateClaimsForUser(string username, IEnumerable<Claim> claims)
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

            var userClaims = await this.userManager.GetClaimsAsync(user);
                        
            foreach (var updatedClaim in claims)
            {
                var existingClaim = userClaims.SingleOrDefault(c => c.Type == updatedClaim.Type);

                if (existingClaim == null)
                {
                    throw new InvalidOperationException("Cannot update claim; there are either none of this type or multiple");
                }

                await this.userManager.ReplaceClaimAsync(user, existingClaim, new System.Security.Claims.Claim(updatedClaim.Type, updatedClaim.Value));
            }
        }

        public async Task DeleteClaimForUser(string username, string claimType, string value)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid user specified");
            }

            var userClaims = await this.userManager.GetClaimsAsync(user);

            var claimToRemove = userClaims.SingleOrDefault(c => c.Type == claimType && c.Value == value);
            if (claimToRemove == null)
            {
                throw new InvalidOperationException("Invalid claim specified");
            }

            await this.userManager.RemoveClaimAsync(user, claimToRemove);
        }
    }
}