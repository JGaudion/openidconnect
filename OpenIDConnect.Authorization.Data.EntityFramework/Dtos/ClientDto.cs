
namespace OpenIDConnect.Authorization.Data.EntityFramework.Dtos
{
    using System.Collections.Generic;
    using System.Linq;        

    using OpenIDConnect.Authorization.Domain.Models;

    public class ClientDto
    {
        private ICollection<GroupDto> groups;

        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string ClaimsUri { get; set; }

        public ICollection<GroupDto> Groups
        {
            get
            {
                return this.groups ?? (this.groups = new List<GroupDto>());
            }

            set
            {
                this.groups = value;
            }
        }

        public Client ToDomainModel()
        {
            return new Client(
                this.Id,
                this.Name,
                this.Enabled,
                this.ClaimsUri);
        }

        public static ClientDto FromDomain(Client client)
        {
            return new ClientDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Enabled = client.Enabled,
                    ClaimsUri = client.ClaimsUri
                };
        }
    }
}