using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OpenIDConnect.Core.Models
{
    public class ExternalIdentity
    {
        public ExternalIdentity(
            IEnumerable<Claim> claims,
            string provider,
            string providerId)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (providerId == null)
            {
                throw new ArgumentNullException(nameof(providerId));
            }

            this.Claims = claims;
            this.Provider = provider;
            this.ProviderId = providerId;
        }

        public IEnumerable<Claim> Claims { get; }

        public string Provider { get; }

        public string ProviderId { get; }
    }
}
