using OpenIDConnect.Users.Domain.Models;

namespace OpenIDConnect.Users.Api.Models
{
    public class UserCreateApiModel
    {
        public string Id { get; set; }

        public string Password { get; set; }

        internal User ToDomainModel()
        {
            return new User(this.Id);
        }
    }
}