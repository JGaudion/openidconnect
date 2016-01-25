using OpenIDConnect.Users.Domain.Models;

namespace OpenIDConnect.Users.Domain
{    
    public interface IUsersRepository
    {
        User GetUser(string userId);
    }
}