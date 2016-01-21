using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Generic;

namespace OpenIDConnect.IdentityAdmin
{
    internal class CompositeAuthenticationHandler : AuthenticationHandler<CompositionAuthenticationOptions>
    {
        private IEnumerable<string> authenticationTypes;

        public CompositeAuthenticationHandler(IEnumerable<string> authenticationTypes)
        {
            if (authenticationTypes == null)
            {
                throw new ArgumentNullException("authenticationTypes");
            }

            this.authenticationTypes = authenticationTypes;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {            
            foreach (var authenticationType in this.authenticationTypes)
            {
                var authenticationResult = await this.Context.Authentication.AuthenticateAsync(authenticationType);
                if (authenticationResult != null)
                {
                    return new AuthenticationTicket(
                        authenticationResult.Identity,
                        authenticationResult.Properties);
                }
            }

            return null;
        }
    }
}