using IdentityServer3.Core.Services;
using System;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

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
            var uri = $"{this.identityAdminUri}/api/clients/{clientId}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.identityAdminUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"admin/api/clients/{clientId}");
                var str = await response.Content.ReadAsStringAsync();                            
            }

            return null;
        }
    }
}