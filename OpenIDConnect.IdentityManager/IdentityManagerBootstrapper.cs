using OpenIDConnect.Core;
using Owin;

namespace OpenIDConnect.IdentityManager
{
    public class IdentityManagerBootstrapper : IOwinBootstrapper
    {        
        public void Run(IAppBuilder app)
        {
            var options = new InMemoryManagerOptionsService().GetManagerOptions();
            app.UseIdentityManager(options);
        }
    }
}