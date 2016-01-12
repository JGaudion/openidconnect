using IdentityAdmin.Configuration;

namespace OpenIDConnect.Host
{
    public interface IAdminOptionsService
    {
        IdentityAdminOptions GetAdminOptions();
    }
}