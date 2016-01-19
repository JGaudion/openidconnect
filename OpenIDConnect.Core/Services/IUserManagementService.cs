using OpenIDConnect.Core.Models.UserManagement;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIDConnect.Core.Services
{
    public interface IUserManagementService
    {
        Task<UserManagementMetadata> GetMetadataAsync();

        // users
        Task<UserManagementResult<string>> CreateUserAsync(string username, string password, IEnumerable<Claim> claims);
        Task<UserManagementResult> DeleteUserAsync(string subject);

        Task<UserManagementResult<QueryResult<UserSummary>>> QueryUsersAsync(string filter, int start, int count);
        Task<UserManagementResult<UserDetail>> GetUserAsync(string subject);

        Task<UserManagementResult> SetUserPropertyAsync(string subject, string type, string value);

        Task<UserManagementResult> AddUserClaimAsync(string subject, string type, string value);
        Task<UserManagementResult> RemoveUserClaimAsync(string subject, string type, string value);

        // roles
        Task<UserManagementResult<string>> CreateRoleAsync(string roleName, IEnumerable<Claim> claims);
        Task<UserManagementResult> DeleteRoleAsync(string subject);

        Task<UserManagementResult<QueryResult<RoleSummary>>> QueryRolesAsync(string filter, int start, int count);
        Task<UserManagementResult<RoleDetail>> GetRoleAsync(string subject);

        Task<UserManagementResult> SetRolePropertyAsync(string subject, string type, string value);
    }
}
