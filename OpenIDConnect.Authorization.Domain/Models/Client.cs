
namespace OpenIDConnect.Authorization.Domain.Models
{
    public class Client
    {
        public Client(string id, string name, bool enabled, string claimsUri)
        {
            this.Id = id;
            this.Name = name;
            this.Enabled = enabled;
            this.ClaimsUri = claimsUri;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public string ClaimsUri { get; }
    }
}