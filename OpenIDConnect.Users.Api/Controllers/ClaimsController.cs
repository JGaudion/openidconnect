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

        [HttpGet("{userId}/claims")]
        public async Task<IActionResult> GetClaim(string userId, [FromQuery] string type = null)
        {
            IEnumerable<Claim> claims =
                string.IsNullOrWhiteSpace(type) ?
                    await this.usersClaimsRepository.GetUserClaims(userId) :
                    await this.usersClaimsRepository.GetUserClaimsOfType(userId, type);

            var claimApiModels =
                claims.Select(c => new ClaimApiModel { Type = c.Type, Value = c.Value });
                
            return this.Ok(claimApiModels);
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
    }
}