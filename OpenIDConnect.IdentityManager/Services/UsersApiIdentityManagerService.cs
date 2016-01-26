using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityManager;

namespace OpenIDConnect.IdentityManager.Services
{
    public class UsersApiIdentityManagerService : IIdentityManagerService
    {
        public Task<IdentityManagerResult> AddUserClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<CreateResult>> CreateRoleAsync(IEnumerable<PropertyValue> properties)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<CreateResult>> CreateUserAsync(IEnumerable<PropertyValue> properties)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> DeleteRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> DeleteUserAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerMetadata> GetMetadataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<RoleDetail>> GetRoleAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<UserDetail>> GetUserAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult<QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> RemoveUserClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> SetRolePropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityManagerResult> SetUserPropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }
    }
}
