
namespace OpenIDConnect.Authorization.Data.EntityFramework.Dtos
{
    using OpenIDConnect.Authorization.Domain.Models;

    public class ClientDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

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