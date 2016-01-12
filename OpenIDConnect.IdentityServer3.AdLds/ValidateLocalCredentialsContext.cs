using System;

namespace OpenIDConnect.IdentityServer3.AdLds
{
    public class ValidateLocalCredentialsContext
    {
        public ValidateLocalCredentialsContext(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            this.UserName = userName;

            this.Password = password;
        }

        public string UserName { get; }

        public string Password { get; }

        public AdLdsUser User { get; set; }
    }
}
