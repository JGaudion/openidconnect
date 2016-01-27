
namespace OpenIDConnect.Authorization.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OpenIDConnect.Authorization.Domain.Models;

    public interface IClientsRepository
    {
        Task<IEnumerable<Client>> GetClients();

        Task<Client> GetClient(string clientId);

        Task Add(Client client);

        Task Update(Client client);

        Task Delete(string clientId);
    }
}