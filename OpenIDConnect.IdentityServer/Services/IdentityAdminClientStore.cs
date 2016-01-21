using IdentityServer3.Core.Services;
using System;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using OpenIDConnect.Core;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Protocols.WSTrust;

namespace OpenIDConnect.IdentityServer.Services
{
    public class IdentityAdminClientStore : IClientStore
    {
        private readonly string identityAdminUri;

        public IdentityAdminClientStore(string identityAdminUri)
        {
            if (string.IsNullOrWhiteSpace(identityAdminUri))
            {
                throw new ArgumentNullException("identityAdminUri");
            }

            this.identityAdminUri = identityAdminUri;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {            
            var clientsUri = $"admin-api/api/clients/{clientId}";

            //var cert = Cert.Load(StoreName.My, StoreLocation.CurrentUser, "b512d01195667dbc7c4222ec6fd563ac64e3d450");
            //var handler = new WebRequestHandler();
            //handler.ClientCertificates.Add(cert);

            // Retrieve an access token from the IdentityAdmin /authorize OAuth endpoint
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.identityAdminUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cert = Cert.Load(typeof(IOwinBootstrapper).Assembly, "Cert", "idsrv3test.pfx", "idsrv3test");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("name", "idServer"),
                        new Claim("role", "IdentityAdminManager"),
                        new Claim("scope", "idadmin-api")
                    }),
                    TokenIssuerName = "idServer",                    
                    AppliesToAddress = this.identityAdminUri,
                    Lifetime = new Lifetime(DateTime.Now, DateTime.Now.AddMinutes(10)),
                    SigningCredentials = new X509SigningCredentials(cert)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(securityToken);

                var jwtParams = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidAudience = this.identityAdminUri,
                    ValidIssuer = "idServer",                    
                    IssuerSigningToken = new X509SecurityToken(cert)                    
                };

                SecurityToken validatedToken;
                tokenHandler.ValidateToken(accessToken, jwtParams, out validatedToken);                

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync(clientsUri);
                var str = await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}