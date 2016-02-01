using OpenIDConnect.Authorization.Domain.Models;
using OpenIDConnect.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<PagingResult<User>> GetUsers(Paging paging);

        Task<IEnumerable<ClientGroup>> GetUserGroups(string userId);

        Task<IEnumerable<Claim>> GetUserClaims(string userId);
    }
}
