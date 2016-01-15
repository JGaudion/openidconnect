using IdentityAdmin.Extensions;
using OpenIDConnect.Core;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Extensions;
using Thinktecture.IdentityModel.Owin;
using System.IdentityModel.Selectors;

namespace OpenIDConnect.IdentityAdmin
{
    public class IdentityAdminBootstrapper : IOwinBootstrapper
    {
        private readonly string identityServerUri;

        private readonly string identityAdminUri;        

        public IdentityAdminBootstrapper(string identityServerUri, string identityAdminUri)
        {
            if (string.IsNullOrWhiteSpace(identityServerUri))
            {
                throw new ArgumentNullException("No identity server uri specified");
            }
            
            if (string.IsNullOrWhiteSpace(identityAdminUri))
            {
                throw new ArgumentNullException("No identity admin uri specified");
            }

            this.identityServerUri = identityServerUri;
            this.identityAdminUri = identityAdminUri;
        }

        public void Run(IAppBuilder app)
        {    
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            
            // If the request has a cookie, then the user is already authenticated, so build the ClaimsIdentity
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"                      
            });

            // This middleware checks the SignInAsAuthenticationType to build the ClaimsIdentity
            // and if not authenticated
            app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = this.identityServerUri,
                ClientId = "idadmin_client",
                RedirectUri = this.identityAdminUri,
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = "openid idadmin",
                SignInAsAuthenticationType = "Cookies",
                Notifications = new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
                            if (result != null)
                            {
                                var id_token = result.Identity.Claims.GetValue("id_token");
                                if (id_token != null)
                                {
                                    n.ProtocolMessage.IdTokenHint = id_token;
                                    n.ProtocolMessage.PostLogoutRedirectUri = this.identityAdminUri;
                                }
                            }
                        }
                    }
                }
            });

            var options = new EntityFrameworkAdminOptionsService().GetAdminOptions();
            app.UseIdentityAdmin(options);
        }
    }
}
