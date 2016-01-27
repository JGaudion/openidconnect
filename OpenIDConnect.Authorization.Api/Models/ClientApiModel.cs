
using OpenIDConnect.Authorization.Domain.Models;

namespace OpenIDConnect.Authorization.Api.Models
{
    using System;

    public class ClientApiModel
    {
        public ClientApiModel()
        {            
        }

        public ClientApiModel(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            this.Id = client.Id;
            this.Name = client.Name;
            this.Enabled = client.Enabled;
            this.ClaimsUri = client.ClaimsUri;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string ClaimsUri { get; set; }
    }
}
