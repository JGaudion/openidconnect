using IdentityAdmin.Configuration;

namespace OpenIDConnect.Host.Interfaces
{
    public interface IAdminOptionsService
    {
        IdentityAdminOptions GetAdminOptions();
    }
}