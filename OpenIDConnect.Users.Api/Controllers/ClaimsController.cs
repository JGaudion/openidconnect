using Microsoft.AspNet.Mvc;
using System;
using OpenIDConnect.Users.Api.Models;
using System.Threading.Tasks;
using OpenIDConnect.Users.Domain.Repositories;
using OpenIDConnect.Users.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Users.Api.Controllers
{    
    [Route("api/users")]
    public class ClaimsController : Controller
    {
        private readonly IUserClaimsRepository usersClaimsRepository;

        public ClaimsController(IUserClaimsRepository usersClaimsRepository)
        {
            if (usersClaimsRepository == null)
            {
                throw new ArgumentNullException(nameof(usersClaimsRepository));
            }

            this.usersClaimsRepository = usersClaimsRepository;
        }

        [HttpPost("{userId}/claims")]
        public async Task<IActionResult> AddClaimToUser(string userId, [FromBody] IEnumerable<ClaimApiModel> claimApiModels)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();   // TODO: unprocessible entity response
            }

            await this.usersClaimsRepository.AddClaimsToUser(
                userId,
                claimApiModels.Select(c => c.ToDomainModel()));

            return this.Ok();       // TODO: return created response
        }


        [HttpGet("{username}/claims")]
        public async Task<IActionResult> GetClaims(string username, [FromQuery]IEnumerable<string> claimTypes)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            IEnumerable<Claim> claims;

            if (claimTypes == null || !claimTypes.Any())
            {
                claims = await this.usersClaimsRepository.GetUserClaims(username);
            }
            else
            {
                claims = await this.usersClaimsRepository.GetUserClaimsOfTypes(username, claimTypes);
            }

            return this.Ok(claims.Select(c => new ClaimApiModel { Type = c.Type, Value = c.Value }));
        }

        [HttpDelete("{username}/claims")]
        public async Task<IActionResult> DeleteClaim(string username, [FromQuery]string claimType, [FromQuery] string value)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            if (string.IsNullOrWhiteSpace(claimType) || value == null)
            {
                //Value can be empty
                return this.HttpBadRequest();
            }

            await this.usersClaimsRepository.DeleteClaimForUser(username, claimType, value);

            return this.Ok();
        }

        [HttpPut("{username}/claims")]
        public async Task<IActionResult> UpdateClaims(string username, [FromBody]IEnumerable<ClaimApiModel> claims)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            await this.usersClaimsRepository.UpdateClaimsForUser(username, claims.Select(c => new Claim(c.Type, c.Value)));

            return this.Ok();
        }

    }
}