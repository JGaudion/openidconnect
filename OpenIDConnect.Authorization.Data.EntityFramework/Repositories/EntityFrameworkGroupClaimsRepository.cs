using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenIDConnect.Authorization.Domain.Models;
using OpenIDConnect.Authorization.Domain.Repositories;
using OpenIDConnect.Authorization.Data.EntityFramework.Context;
using System.Linq;
using Microsoft.Data.Entity;
using OpenIDConnect.Authorization.Data.EntityFramework.Dtos;

namespace OpenIDConnect.Authorization.Data.EntityFramework.Repositories
{
    public class EntityFrameworkGroupClaimsRepository : IGroupClaimsRepository
    {
        private readonly AuthorizationDbContext context;

        public EntityFrameworkGroupClaimsRepository(AuthorizationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        public async Task<string> AddClaim(string clientId, string groupId, GroupClaim claim)
        {
            var group = await this.context.Groups
                .Include(g => g.GroupClaims)
                .FirstOrDefaultAsync(g => g.ClientId == clientId && g.Id == int.Parse(groupId));

            if (group == null)
            {
                throw new InvalidOperationException($"Could not find group {groupId} in client {clientId}");
            }

            var groupClaimDto = new GroupClaimDto { Type = claim.Type, Value = claim.Value, GroupId = int.Parse(groupId) };

            group.GroupClaims.Add(groupClaimDto);

            await this.context.SaveChangesAsync();

            return groupClaimDto.Id.ToString();
        }

        public async Task DeleteClaim(string clientId, string groupId, string claimId)
        {
            var group = await this.context.Groups
                .Include(g => g.GroupClaims)
                .FirstOrDefaultAsync(g => g.ClientId == clientId && g.Id == int.Parse(groupId));

            if (group == null)
            {
                throw new InvalidOperationException($"Could not find group {groupId} in client {clientId}");
            }

            var claim = group.GroupClaims.FirstOrDefault(c => c.Id == int.Parse(claimId));

            if (claim == null)
            {
                throw new InvalidOperationException($"Could not find claim {claimId} in group {groupId} in client {clientId}");
            }

            group.GroupClaims.Remove(claim);

            await this.context.SaveChangesAsync();
        }

        public async Task<GroupClaim> GetClaim(string clientId, string groupId, string claimId)
        {
            var group = await this.context.Groups
                            .Include(g => g.GroupClaims)
                            .FirstOrDefaultAsync(g => g.ClientId == clientId && g.Id == int.Parse(groupId));

            if (group == null)
            {
                throw new InvalidOperationException($"Could not find group {groupId} in client {clientId}");
            }

            var claim = group.GroupClaims.FirstOrDefault(c => c.Id == int.Parse(claimId));

            if (claim == null)
            {
                throw new InvalidOperationException($"Could not find claim {claimId} in group {groupId} in client {clientId}");
            }

            return claim.ToDomain();
        }

        public async Task<IEnumerable<GroupClaim>> GetClaims(string clientId, string groupId)
        {
            var group = await this.context.Groups
                            .Where(g => g.ClientId == clientId && g.Id == int.Parse(groupId))
                            .Include(g => g.GroupClaims)
                            .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new InvalidOperationException($"Could not find group {groupId} in client {clientId}");
            }

            return group.GroupClaims.Select(c => c.ToDomain());
        }

        public async Task UpdateClaim(string clientId, string groupId, GroupClaim claim)
        {
            var group = await this.context.Groups
                            .Include(g => g.GroupClaims)
                            .FirstOrDefaultAsync(g => g.ClientId == clientId && g.Id == int.Parse(groupId));

            if (group == null)
            {
                throw new InvalidOperationException($"Could not find group {groupId} in client {clientId}");
            }

            var claimDto = group.GroupClaims.FirstOrDefault(c => c.Id == int.Parse(claim.Id));

            if (claimDto == null)
            {
                throw new InvalidOperationException($"Could not find claim {claim.Id} in group {groupId} in client {clientId}");
            }

            claimDto.Type = claim.Type;
            claimDto.Value = claim.Value;

            await this.context.SaveChangesAsync();
        }
    }
}
