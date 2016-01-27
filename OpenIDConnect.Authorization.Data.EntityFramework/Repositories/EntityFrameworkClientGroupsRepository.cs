using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Data.EntityFramework.Repositories
{
    using Microsoft.Data.Entity;

    using OpenIDConnect.Authorization.Data.EntityFramework.Context;
    using OpenIDConnect.Authorization.Data.EntityFramework.Dtos;
    using OpenIDConnect.Authorization.Domain.Models;
    using OpenIDConnect.Authorization.Domain.Repositories;

    public class EntityFrameworkClientGroupsRepository : IClientGroupsRepository
    {
        private readonly AuthorizationDbContext context;

        public EntityFrameworkClientGroupsRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Group>> GetGroups(string clientId)
        {
            try
            {
                var client =
                  await this.context.Clients
                      .Where(c => c.Id == clientId)
                      .Include(c => c.Groups)
                      .FirstOrDefaultAsync();

                if (client == null)
                {
                    throw new InvalidOperationException("Invalid client specified");
                }

                return client.Groups.Select(
                    g => new Group(g.Id.ToString(), g.Name));
            }
            catch (Exception exception)
            {
                var x = exception.ToString();
                throw;
            }
        }

        public async Task AddGroup(string clientId, Group group)
        {
            var groupDto = new GroupDto
            {
                Name = group.Name,
                ClientId = clientId
            };

            this.context.Groups.Add(groupDto);
            await this.context.SaveChangesAsync();
        }

        public async Task<Group> GetGroup(string clientId, string groupId)
        {
            // TODO: no Find equivalent in EF7, can't explicitly Load related data with filter
            var client =
              await this.context.Clients
                  .Where(c => c.Id == clientId)
                  .Include(c => c.Groups)
                  .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new InvalidOperationException("Invalid client specified");
            }

            var group = client.Groups.FirstOrDefault(g => g.Id == int.Parse(groupId));
            return group?.ToDomainModel();
        }
    }
}
