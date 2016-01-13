
using Owin;

namespace OpenIDConnect.Core
{
    public interface IOwinBootstrapper
    {
        void Run(IAppBuilder app);
    }
}
