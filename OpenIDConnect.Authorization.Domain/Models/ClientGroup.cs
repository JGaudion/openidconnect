namespace OpenIDConnect.Authorization.Domain.Models
{
    public class ClientGroup
    {
        public ClientGroup(string clientId, string groupId)
        {
            this.ClientId = clientId;
            this.Id = groupId;
        }

        public string ClientId { get; }

        public string Id { get; }
    }
}
