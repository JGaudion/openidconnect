using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Mvc;
using OpenIDConnect.Authorization.Api.Models;
using OpenIDConnect.Authorization.Domain.Models;
using OpenIDConnect.Authorization.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Api.Controllers
{
    [EnableCors("cors_policy")]
    [Route("api/clients")]
    public class ClaimsController : Controller
    {
        private readonly IGroupClaimsRepository groupClaimsRepository;

        public ClaimsController(IGroupClaimsRepository groupClaimsRepository)
        {
            if (groupClaimsRepository == null)
            {
                throw new ArgumentNullException(nameof(groupClaimsRepository));
            }

            this.groupClaimsRepository = groupClaimsRepository;
        }

        [HttpGet("{clientId}/groups/{groupId}/claims")]
        public async Task<IActionResult> GetClaims(string clientId, string groupId)
        {
            var result = await this.groupClaimsRepository.GetClaims(clientId, groupId);

            return this.Ok(result.Select(c => new GroupClaimApiModel { Id = c.Id, Type = c.Type, Value = c.Value }));
        }

        [HttpPost("{clientId}/groups/{groupId}/claims")]
        public async Task<IActionResult> AddClaim(string clientId, string groupId, [FromBody] GroupClaimApiModel claim)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            claim.Id = await this.groupClaimsRepository.AddClaim(clientId, groupId, claim.ToDomain());

            return this.Ok(claim);
        }

        [HttpGet("{clientId}/groups/{groupId}/claims/{claimId}")]
        public async Task<IActionResult> GetClaim(string clientId, string groupId, string claimId)
        {
            var result = await this.groupClaimsRepository.GetClaim(clientId, groupId, claimId);

            return this.Ok(new GroupClaimApiModel { Id = result.Id, Type = result.Type, Value = result.Value });
        }

        [HttpPut("{clientId}/groups/{groupId}/claims/{claimId}")]
        public async Task<IActionResult> UpdateClaim(string clientId, string groupId, string claimId, [FromBody] GroupClaimApiModel claim)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            await this.groupClaimsRepository.UpdateClaim(clientId, groupId, new GroupClaim(claimId, claim.Type, claim.Value));

            return this.Ok();
        }

        [HttpDelete("{clientId}/groups/{groupId}/claims/{claimId}")]
        public async Task<IActionResult> DeleteClaim(string clientId, string groupId, string claimId)
        {
            await this.groupClaimsRepository.DeleteClaim(clientId, groupId, claimId);

            return this.Ok();
        }
    }
}
