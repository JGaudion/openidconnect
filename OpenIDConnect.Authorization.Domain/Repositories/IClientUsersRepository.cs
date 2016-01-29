namespace OpenIDConnect.Authorization.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OpenIDConnect.Authorization.Domain.Models;

    public interface IClientUsersRepository
    {
        Task<IEnumerable<User>> Get(string clientId, string groupId);
    }
}