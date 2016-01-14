using IdentityServer3.Core.Configuration;

namespace OpenIDConnect.Host.Interfaces
{
    public interface IServerOptionsService
    {
        IdentityServerOptions GetServerOptions();
    }
}