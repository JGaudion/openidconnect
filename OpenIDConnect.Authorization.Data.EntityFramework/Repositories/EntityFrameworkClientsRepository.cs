
namespace OpenIDConnect.Authorization.Data.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Data.Entity;

    using OpenIDConnect.Authorization.Data.EntityFramework.Context;
    using OpenIDConnect.Authorization.Data.EntityFramework.Dtos;
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

        public async Task<Client> GetClient(string clientId)
        {
            var client = await this.context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            return client?.ToDomainModel();
        }

        public async Task Add(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var clientDto = ClientDto.FromDomain(client);
            this.context.Clients.Add(clientDto);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var clientDto = ClientDto.FromDomain(client);
            this.context.Clients.Update(clientDto);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var clientDto = new ClientDto { Id = clientId };
            this.context.Clients.Attach(clientDto);
            this.context.Clients.Remove(clientDto);
            await this.context.SaveChangesAsync();
        }
    }
}