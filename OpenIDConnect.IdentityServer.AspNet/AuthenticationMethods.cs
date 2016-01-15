using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet
{
    public static class AuthenticationMethods
    {
        public const string Certificate = "certificate";
        public const string External = "external";
        public const string Password = "password";
        public const string TwoFactorAuthentication = "2fa";
    }
}
