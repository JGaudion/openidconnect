using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using IdentityServer3.Admin.EntityFramework;
using IdentityServer3.Admin.EntityFramework.Entities;

namespace OpenIDConnect.IdentityAdmin
{
    internal class EntityFrameworkAdminOptionsService
    {
        public EntityFrameworkAdminOptionsService()
        {
        }

        public IdentityAdminOptions GetAdminOptions()
        {
            var factory = new IdentityAdminServiceFactory();
            factory.Configure();
            return new IdentityAdminOptions
            {
                Factory = factory
            };
        }
    }

    internal static class IdentityAdminServiceExtensions
    {
        public static void Configure(this IdentityAdminServiceFactory factory)
        {
            factory.IdentityAdminService = new Registration<IIdentityAdminService, IdentityAdminManagerService>();
        }
    }

    internal class IdentityAdminManagerService : IdentityAdminCoreManager<IdentityClient, int, IdentityScope, int>
    {
        public IdentityAdminManagerService() : base("IdentityAdmin")
        {
        }
    }
}