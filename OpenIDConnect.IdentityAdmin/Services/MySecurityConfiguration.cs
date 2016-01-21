using System.Security.Cryptography.X509Certificates;
using IdentityAdmin.Configuration;
using Owin;

namespace OpenIDConnect.IdentityAdmin
{
    internal class MySecurityConfiguration : AdminHostSecurityConfiguration
    {              
        public X509Certificate2 SigningCert { get; set; }

        public override void Configure(IAppBuilder app)
        {
            base.Configure(app);

            var externalBearerTokenConfiguration = new ExternalBearerTokenConfiguration()
            {
                NameClaimType = this.NameClaimType,
                RoleClaimType = this.RoleClaimType,
                AdminRoleName = this.AdminRoleName,
                SigningCert = this.SigningCert
            };

            externalBearerTokenConfiguration.Configure(app);
        }
    }
}