using OpenIDConnect.Core;
using Owin;

namespace OpenIDConnect.IdentityAdmin
{
    public class IdentityAdminBootstrapper : IOwinBootstrapper
    {
        public void Run(IAppBuilder app)
        {
            var options = new EntityFrameworkAdminOptionsService().GetAdminOptions();
            app.UseIdentityAdmin(options);
        }
    }
}
