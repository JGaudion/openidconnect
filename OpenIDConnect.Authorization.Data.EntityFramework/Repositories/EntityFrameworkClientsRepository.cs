
namespace OpenIDConnect.Authorization.Data.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Data.Entity;

    using OpenIDConnect.Authorization.Data.EntityFramework.Context;
    using OpenIDConnect.Authorization.Domain.Models;
    using OpenIDConnect.Authorization.Domain.Repositories;

    public class EntityFrameworkClientsRepository : IClientsRepository
    {
        private readonly AuthorizationDbContext context;

        public EntityFrameworkClientsRepository(AuthorizationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            var clients = await this.context.Clients.ToListAsync();
            return clients.Select(c => c.ToDomainModel()).ToList();
        }

        public Task<Client> GetClient(string clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}
