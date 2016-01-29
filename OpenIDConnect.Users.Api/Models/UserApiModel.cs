namespace OpenIDConnect.Users.Api.Models
{
    using OpenIDConnect.Users.Domain.Models;

    public class UserApiModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public static UserApiModel FromDomainModel(User user)
        {
            return new UserApiModel
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}
