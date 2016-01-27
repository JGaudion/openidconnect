
using OpenIDConnect.Authorization.Domain.Models;

namespace OpenIDConnect.Authorization.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public string ClaimsUri { get; set; }

        public Client ToDomainModel()
        {
            return new Client(
                this.Id, 
                this.Name, 
                this.Enabled, 
                this.ClaimsUri);
        }
    }
}