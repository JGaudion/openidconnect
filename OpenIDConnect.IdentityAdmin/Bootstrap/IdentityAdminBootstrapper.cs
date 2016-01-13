using IdentityAdmin.Extensions;
using OpenIDConnect.Core;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityAdmin
{
    public class IdentityAdminBootstrapper : IOwinBootstrapper
    {
        private readonly IConfigurationService configurationService;

        public IdentityAdminBootstrapper(IConfigurationService configurationService)
        {
            if (configurationService == null)
            {
                throw new ArgumentNullException("configurationService");
            }

            this.configurationService = configurationService;
        }

        public void Run(IAppBuilder app)
        {
            var identityServerUri = this.configurationService.GetSetting<string>("IdentityServerUri", null);
            if (string.IsNullOrWhiteSpace(identityServerUri))
            {
                throw new InvalidOperationException("No identity server uri specified");
            }

            var identityAdminUri = this.configurationService.GetSetting<string>("IdentityAdminUri", null);
            if (string.IsNullOrWhiteSpace(identityAdminUri))
            {
                throw new InvalidOperationException("No identity admin uri specified");
            }

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
            });

            app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = identityServerUri,
                ClientId = "idadmin_client",
                RedirectUri = identityAdminUri,
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
                                    n.ProtocolMessage.PostLogoutRedirectUri = identityAdminUri;
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
