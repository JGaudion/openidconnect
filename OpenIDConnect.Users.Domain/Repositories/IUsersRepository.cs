using OpenIDConnect.Users.Domain.Models;
using System.Threading.Tasks;

namespace OpenIDConnect.Users.Domain
{    
    public interface IUsersRepository
    {
        Task<User> GetUser(string userId);

        Task AddUser(User user);
        
        Task<bool> Authenticate(string userId, string password);
    }
}