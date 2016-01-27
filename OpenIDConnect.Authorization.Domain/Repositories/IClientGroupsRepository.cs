namespace OpenIDConnect.Authorization.Domain.Repositories
{
    using System.Collections.Generic;    
    using System.Threading.Tasks;

    using OpenIDConnect.Authorization.Domain.Models;

    public interface IClientGroupsRepository
    {
        Task<IEnumerable<Group>> GetGroups(string clientId);

        Task AddGroup(string clientId, Group group);

        Task<Group> GetGroup(string clientId, string groupId);
    }
}