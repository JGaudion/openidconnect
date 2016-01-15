using OpenIDConnect.IdentityServer.AspNet.Model;

namespace OpenIDConnect.IdentityServer.AspNet.Services
{
    public class ConcreteAspNetUserService : AspNetUserService<User, string>
    {
        public ConcreteAspNetUserService(UserManager manager)
            : base(manager)
        {

        }
    }
}
