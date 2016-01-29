using System;

namespace OpenIDConnect.Authorization.Domain.Models
{
    public class GroupClaim
    {
        public GroupClaim(string id, string type, string value)
        {
            this.Id = id;
            this.Type = type;
            this.Value = value;
        }

        public string Id { get; }

        public string Type { get; }

        public string Value { get; }
    }
}
