﻿using IdentityAdmin.Extensions;
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
using Microsoft.Owin.Security;

namespace OpenIDConnect.IdentityAdmin
{
    public class IdentityAdminBootstrapper : IOwinBootstrapper
    {
        private readonly string identityServerUri;

        private readonly string identityAdminUri;

        private readonly bool apiOnly;

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
            this.apiOnly = false;
        }

        public void Run(IAppBuilder app)
        {    
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            //app.UseCompositeAuthentication(new CompositionAuthenticationOptions
            //{
            //    AuthenticationType = "Composite",
            //    CompositeAuthenticationTypes = new string[]
            //    {
            //        "Certificate",
            //        "Cookies"
            //    }
            //});

            //app.UseClientCertificateAuthentication(new ClientCertificateAuthenticationOptions
            //{
            //    AuthenticationType = "Certificate",
            //    AuthenticationMode = AuthenticationMode.Passive,
            //    Validator = new TestCertificateValidator("issuer")
            //});

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {                
                AuthenticationType = "Cookies"
            });

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
                        // Add the returned id token from the IdP to the returned ticket so that 
                        // it can be retrieved later when we logout (see below)
                        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        // If we redirect to the IdP to perform a logout, then 
                        // before the redirect clear the associated cookie, and send the id_token as a hint
                        // so that the user name can be displayed by the IdP
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

            var options = new EntityFrameworkAdminOptionsService(
                this.apiOnly).GetAdminOptions();

            app.UseIdentityAdmin(options);
        }
    }

    internal static class CompositeAuthenticationExtensions
    {
        public static IAppBuilder UseCompositeAuthentication(this IAppBuilder app, CompositionAuthenticationOptions options)
        {
            return app.Use(typeof(CompositeAuthenticationMiddleware), app, options);
        }
    }
}