using OpenIDConnect.AdLds.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.AdLds.Contexts
{
    public interface IDirectoryContext
    {
        Task<AdLdsUser> ValidateCredentialsAsync(string username, string password);

        Task<AdLdsUser> FindUserByNameAsync(string username);

        Task<AdLdsUser> FindUserByLinkedAccountAsync(string provider, string providerId);

        Task<string> CreateUserAsync(string username, string password, IEnumerable<Claim> claims);

        Task<AdLdsUser> CreateLinkedUserAsync(string provider, string providerId, IEnumerable<Claim> claims);

        Task<QueryResult<AdLdsUser>> QueryUsersAsync(int start, int count);

        Task<QueryResult<AdLdsRole>> QueryRolesAsync(int start, int count);

    }
}
