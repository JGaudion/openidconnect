using IdentityServer3.Core.Configuration;

namespace OpenIDConnect.Host
{
    internal interface IServerOptionsService
    {
        IdentityServerOptions GetServerOptions();
    }
}