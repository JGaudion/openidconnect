using IdentityManager.Configuration;

namespace OpenIDConnect.Host.Interfaces
{
    public interface IManagerOptionsService
    {
        IdentityManagerOptions GetManagerOptions();
    }
}
