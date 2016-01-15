
using System;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using System.Collections.Generic;

namespace OpenIDConnect.IdentityServer.Services
{
    public class CompositeClientStore : IClientStore
    {
        private readonly IEnumerable<IClientStore> clientStores;

        public CompositeClientStore(IEnumerable<IClientStore> clientStores)
        {
            if (clientStores == null)
            {
                throw new ArgumentNullException("clientStores");
            }

            this.clientStores = clientStores;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            foreach (var clientStore in this.clientStores)
            {
                var client = await clientStore.FindClientByIdAsync(clientId);
                if (client != null)
                {
                    return client;
                }
            }

            return null;
        }
    }
}