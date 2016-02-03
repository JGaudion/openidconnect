using OpenIDConnect.Authorization.Domain.Models;
using System;

namespace OpenIDConnect.Authorization.Data.EntityFramework.Dtos
{
    public class GroupClaimDto
    {
        public GroupClaimDto()
        {

        }

        public int Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public int GroupId { get; set; }

        public GroupDto Group { get; set; }

        public GroupClaim ToDomain()
        {
            return new GroupClaim(this.Id.ToString(), this.Type, this.Value);
        }
    }
}
