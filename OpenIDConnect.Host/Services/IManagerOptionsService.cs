using IdentityManager.Configuration;

namespace OpenIDConnect.Host
{
    public interface IManagerOptionsService
    {
        IdentityManagerOptions GetManagerOptions();
    }
}
