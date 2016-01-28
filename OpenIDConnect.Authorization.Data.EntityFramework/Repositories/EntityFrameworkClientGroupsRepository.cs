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

        public async Task Update(string clientId, Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var groupDto = GroupDto.FromDomain(group);
            groupDto.ClientId = clientId;

            this.context.Groups.Update(groupDto);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(string clientId, string groupId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentNullException(nameof(groupId));
            }

            var groupDto = new GroupDto { Id = int.Parse(groupId) };
            this.context.Groups.Attach(groupDto);
            this.context.Groups.Remove(groupDto);
            await this.context.SaveChangesAsync();
        }
    }
}