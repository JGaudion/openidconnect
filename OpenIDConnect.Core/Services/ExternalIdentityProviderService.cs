using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using System;
using System.Collections.Generic;

namespace OpenIDConnect.Core.Services
{
    public class ExternalIdentityProviderService
    {
        private readonly List<Action<IAppBuilder, string>> configurators = new List<Action<IAppBuilder, string>>();

        public ExternalIdentityProviderService WithGoogleAuthentication(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            configurators.Add((appBuilder, signInAsType) =>
            {
                var google = new GoogleOAuth2AuthenticationOptions
                {
                    AuthenticationType = "Google",
                    SignInAsAuthenticationType = signInAsType,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };
                appBuilder.UseGoogleAuthentication(google);
            });

            return this;
        }

        public ExternalIdentityProviderService WithTwitterAuthentication(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            configurators.Add((appBuilder, signInAsType) =>
            {
                var twitter = new TwitterAuthenticationOptions
                {
                    AuthenticationType = "Twitter",
                    SignInAsAuthenticationType = signInAsType,
                    ConsumerKey = clientId,
                    ConsumerSecret = clientSecret
                };
                appBuilder.UseTwitterAuthentication(twitter);
            });

            return this;
        }

        public ExternalIdentityProviderService WithFacebookAuthentication(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            configurators.Add((appBuilder, signInAsType) =>
            {
                var fb = new FacebookAuthenticationOptions
                {
                    AuthenticationType = "Facebook",
                    SignInAsAuthenticationType = signInAsType,
                    AppId = clientId,
                    AppSecret = clientSecret
                };

                appBuilder.UseFacebookAuthentication(fb);
            });

            return this;
        }

        public void UseExternalIdentityProviders(IAppBuilder appBuilder, string signInType)
        {
            foreach (var configurator in configurators)
            {
                configurator(appBuilder, signInType);
            }
        }
    }
}
