using IdentityServer3.Core.Configuration;

namespace OpenIDConnect.Host
{
    public interface IServerOptionsService
    {
        IdentityServerOptions GetServerOptions();
    }
}