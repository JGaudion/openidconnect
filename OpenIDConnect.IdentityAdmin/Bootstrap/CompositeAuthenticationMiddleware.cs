using System;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin;
using Owin;

namespace OpenIDConnect.IdentityAdmin
{
    internal class CompositeAuthenticationMiddleware : AuthenticationMiddleware<CompositionAuthenticationOptions>
    {
        public CompositeAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, CompositionAuthenticationOptions options) : base(next, options)
        {
            // TODO: add logging to app?
        }

        protected override AuthenticationHandler<CompositionAuthenticationOptions> CreateHandler()
        {
            return new CompositeAuthenticationHandler(this.Options.CompositeAuthenticationTypes);
        }
    }
}