using OpenIDConnect.Users.Domain;
using OpenIDConnect.Users.Domain.Models;

namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    public class AspNetIdentityUsersRepository : IUsersRepository
    {
        public User GetUser(string userId)
        {
            return new User("test");
        }
    }
}