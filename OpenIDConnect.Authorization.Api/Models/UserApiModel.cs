
namespace OpenIDConnect.Authorization.Api.Models
{
    using OpenIDConnect.Authorization.Domain.Models;

    public class UserApiModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public static UserApiModel FromDomain(User user)
        {
            return new UserApiModel
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        public User ToDomain()
        {
            return new User(this.Id, this.Username);
        }
    }
}
