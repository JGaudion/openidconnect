using OpenIDConnect.Core.Domain.Models;
using OpenIDConnect.Users.Domain.Models;
using System.Threading.Tasks;

namespace OpenIDConnect.Users.Domain
{    
    public interface IUsersRepository
    {
        Task<User> GetUserByName(string username);

        Task AddUser(User user);

        Task UpdateUser(User user);

        Task DeleteUser(string username);
        
        Task<bool> Authenticate(string username, string password);

        Task<PagingResult<User>> QueryUsers(string username, Paging paging);
    }
}