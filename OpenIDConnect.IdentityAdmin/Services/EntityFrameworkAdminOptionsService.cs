using System;
using IdentityAdmin.Configuration;
using IdentityAdmin.Core;
using IdentityServer3.Admin.EntityFramework;
using IdentityServer3.Admin.EntityFramework.Entities;
using OpenIDConnect.Core;
using System.Linq;

namespace OpenIDConnect.IdentityAdmin
{
    internal class EntityFrameworkAdminOptionsService
    {
        private readonly bool apiOnly;

        public EntityFrameworkAdminOptionsService(bool apiOnly)
        {
            this.apiOnly = apiOnly;
        }

        public IdentityAdminOptions GetAdminOptions()
        {
            var factory = new IdentityAdminServiceFactory();
            factory.Configure();
            return new IdentityAdminOptions
            {
                Factory = factory,
                AdminSecurityConfiguration = GetSecurityConfiguration(this.apiOnly),
                DisableUserInterface = this.apiOnly
                //AdminSecurityConfiguration = new MySecurityConfiguration()
                //{
                //    HostAuthenticationType = "Cookies",
                //    NameClaimType = "name",
                //    RoleClaimType = "role",
                //    AdminRoleName = "IdentityAdminManager",
                //    SigningCert = Cert.Load(typeof(IOwinBootstrapper).Assembly, "Cert", "idsrv3test.pfx", "idsrv3test")
                //}
            };
        }

        private AdminSecurityConfiguration GetSecurityConfiguration(bool useExternalAccessToken)
        {
            if (useExternalAccessToken)
            {
                return new ExternalBearerTokenConfiguration()
                {
                    Audience = "https://localhost:44302/admin",     // TODO: get from config
                    Issuer = "idServer",
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    AdminRoleName = "IdentityAdminManager",
                    SigningCert = Cert.Load(typeof(IOwinBootstrapper).Assembly, "Cert", "idsrv3test.pfx", "idsrv3test"),
                    Scope = "idadmin-api"
                };
            }

            return new AdminHostSecurityConfiguration
            {
                HostAuthenticationType = "Cookies",
                NameClaimType = "name",
                RoleClaimType = "role",
                AdminRoleName = "IdentityAdminManager"
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