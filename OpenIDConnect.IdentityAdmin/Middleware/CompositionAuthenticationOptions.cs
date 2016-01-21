
using System.Collections.Generic;
using Microsoft.Owin.Security;
using System.Linq;

namespace OpenIDConnect.IdentityAdmin
{
    public class CompositionAuthenticationOptions : AuthenticationOptions
    {
        private IEnumerable<string> compositeAuthenticationTypes;

        public CompositionAuthenticationOptions() : base("Composite")
        {
            AuthenticationMode = AuthenticationMode.Passive;
        }

        public CompositionAuthenticationOptions(string authenticationType) : base(authenticationType)
        {
        }

        public IEnumerable<string> CompositeAuthenticationTypes
        {
            get
            {
                return this.compositeAuthenticationTypes ?? (this.compositeAuthenticationTypes = Enumerable.Empty<string>());
            }

            set
            {
                this.compositeAuthenticationTypes = value;
            }
        }
    }
}