using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace OpenIDConnect.IdentityAdmin
{
    internal class TestCertificateValidator : X509CertificateValidator
    {
        private readonly string issuer;

        public TestCertificateValidator(string issuer)
        {
            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentNullException("issuer");
            }

            this.issuer = issuer;
        }

        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new SecurityTokenValidationException
                    ("Certificate is invalid");
            }

            if (string.Compare(this.issuer, certificate.IssuerName.Name, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new SecurityTokenValidationException
                    ("Certificate was not issued by a trusted issuer");
            }
        }
    }
}