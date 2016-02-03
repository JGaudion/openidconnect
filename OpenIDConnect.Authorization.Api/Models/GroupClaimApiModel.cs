using OpenIDConnect.Authorization.Domain.Models;

namespace OpenIDConnect.Authorization.Api.Models
{
    public class GroupClaimApiModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public GroupClaim ToDomain()
        {
            return new GroupClaim(this.Id, this.Type, this.Value);
        }
    }
}
