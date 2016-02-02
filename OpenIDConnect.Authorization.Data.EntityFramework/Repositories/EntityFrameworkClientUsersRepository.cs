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

    public class EntityFrameworkClientUsersRepository : IClientUsersRepository
    {
        private readonly AuthorizationDbContext context;

        public EntityFrameworkClientUsersRepository(AuthorizationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        } 

        public async Task<IEnumerable<User>> Get(string clientId, string groupId)
        {
            var group =
                await
                this.context.Clients.Where(c => c.Id == clientId)
                    .SelectMany(c => c.Groups)
                    .Where(g => g.Id.ToString() == groupId)
                    .Include(g => g.Users)
                    .FirstOrDefaultAsync();

            return group?.Users.Select(u => u.ToDomainModel());
        }
    }
}