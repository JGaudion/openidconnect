using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpenIDConnect.Core.Models
{
    public class ProfileDataRequest
    {
        public ProfileDataRequest(
            ClaimsPrincipal subject,
            Client client,
            string caller,
            IEnumerable<string> requestedClaimTypes
            )
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (string.IsNullOrEmpty(caller))
            {
                throw new ArgumentNullException(nameof(caller));
            }

            if (requestedClaimTypes == null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            this.Subject = subject;
            this.Client = client;
            this.Caller = caller;
            this.RequestedClaimTypes = requestedClaimTypes;
        }

        public ProfileDataRequest(
            ClaimsPrincipal subject,
            Client client,
            string caller
            )
            : this(subject, client, caller, Enumerable.Empty<string>())
        {
            this.AllClaimsRequested = true;
        }

        public bool AllClaimsRequested { get; }
        public string Caller { get; }
        public Client Client { get; }
        public IEnumerable<string> RequestedClaimTypes { get; }
        public ClaimsPrincipal Subject { get; }
    }
}
