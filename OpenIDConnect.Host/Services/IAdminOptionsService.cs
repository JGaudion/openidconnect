using IdentityAdmin.Configuration;

namespace OpenIDConnect.Host
{
    internal interface IAdminOptionsService
    {
        IdentityAdminOptions GetAdminOptions();
    }
}