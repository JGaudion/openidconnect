using IdentityManager.Extensions;
using OpenIDConnect.Core;
using OpenIDConnect.Core.Services;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityManager
{
    public class IdentityManagerBootstrapper : IOwinBootstrapper
    {
        private readonly string identityServerUri;
        private readonly string identityManagerUri;
        private readonly IUserManagementService userManagementService;

        public IdentityManagerBootstrapper(IUserManagementService userManagementService, string identityServerUri, string identityManagerUri)
        {
            if (userManagementService == null)
            {
                throw new ArgumentNullException(nameof(userManagementService));
            }

            if (string.IsNullOrWhiteSpace(identityServerUri))
            {
                throw new ArgumentNullException("No identity server uri specified");
            }

            if (string.IsNullOrWhiteSpace(identityManagerUri))
            {
                throw new ArgumentNullException("No identity manager uri specified");
            }

            this.userManagementService = userManagementService;
            this.identityServerUri = identityServerUri;
            this.identityManagerUri = identityManagerUri;
        }

        public void Run(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
            });

            app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                Authority = this.identityServerUri,
                ClientId = "idmanager_client",
                RedirectUri = this.identityManagerUri,
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Scope = "openid idmanager",
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
                                    n.ProtocolMessage.PostLogoutRedirectUri = this.identityManagerUri;
                                }
                            }
                        }
                    }
                }
            });

            var options = new InMemoryManagerOptionsService(this.userManagementService).GetManagerOptions();
            app.UseIdentityManager(options);
        }
    }
}