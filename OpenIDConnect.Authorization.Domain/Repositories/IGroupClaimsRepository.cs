using OpenIDConnect.Authorization.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Domain.Repositories
{
    public interface IGroupClaimsRepository
    {
        Task<GroupClaim> GetClaim(string clientId, string groupId, string claimId);

        Task<IEnumerable<GroupClaim>> GetClaims(string clientId, string groupId);

        Task<string> AddClaim(string clientId, string groupId, GroupClaim claim);

        Task UpdateClaim(string clientId, string groupId, GroupClaim claim);

        Task DeleteClaim(string clientId, string groupId, string claimId);
    }
}
