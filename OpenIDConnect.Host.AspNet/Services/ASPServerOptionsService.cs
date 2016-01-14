using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using OpenIDConnect.Host.AspNet.IdSvr;

namespace OpenIDConnect.Host.AspNet.Services
{
    public class ASPServerOptionsService
    {
        public IdentityServerOptions GetServerOptions()
        {
            var myFactory = Factory.Configure();
            myFactory.ConfigureUserService("SnakesDatabase");

            return new IdentityServerOptions
            {
                SiteName = "Steam Weasel", //Does it matter?
                SigningCertificate = Certificate.Get(), //Do we need this?
                Factory = myFactory,
                //EXAMPLE for extra providers
                //AuthenticationOptions = new AuthenticationOptions
                //{
                //    IdentityProviders = ConfigureAdditionalIdentityProviders,
                //}
            };
        }

        //public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        //{
        //    var google = new GoogleOAuth2AuthenticationOptions
        //    {
        //        AuthenticationType = "Google",
        //        SignInAsAuthenticationType = signInAsType,
        //        ClientId = "767400843187-8boio83mb57ruogr9af9ut09fkg56b27.apps.googleusercontent.com",
        //        ClientSecret = "5fWcBT0udKY7_b6E3gEiJlze"
        //    };
        //    app.UseGoogleAuthentication(google);

        //    var fb = new FacebookAuthenticationOptions
        //    {
        //        AuthenticationType = "Facebook",
        //        SignInAsAuthenticationType = signInAsType,
        //        AppId = "676607329068058",
        //        AppSecret = "9d6ab75f921942e61fb43a9b1fc25c63"
        //    };
        //    app.UseFacebookAuthentication(fb);

        //    var twitter = new TwitterAuthenticationOptions
        //    {
        //        AuthenticationType = "Twitter",
        //        SignInAsAuthenticationType = signInAsType,
        //        ConsumerKey = "N8r8w7PIepwtZZwtH066kMlmq",
        //        ConsumerSecret = "df15L2x6kNI50E4PYcHS0ImBQlcGIt6huET8gQN41VFpUCwNjM"
        //    };
        //    app.UseTwitterAuthentication(twitter);
        //}
    }
}
