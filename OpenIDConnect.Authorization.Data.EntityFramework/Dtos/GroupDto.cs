namespace OpenIDConnect.Authorization.Data.EntityFramework.Dtos
{
    using OpenIDConnect.Authorization.Domain.Models;
    using System.Collections.Generic;
    public class GroupDto
    {
        private ICollection<GroupClaimDto> groupClaims = null;

        public int Id { get; set; }

        public string Name { get; set; }

        public string ClientId { get; set; }

        public ClientDto Client { get; set; }

        public ICollection<GroupClaimDto> GroupClaims
        {
            get
            {
                return this.groupClaims ?? (this.groupClaims = new List<GroupClaimDto>());
            }
            set
            {
                this.groupClaims = value;
            }
        }

        public Group ToDomainModel()
        {
            return new Group(this.Id.ToString(), this.Name);
        }

        public static GroupDto FromDomain(Group group)
        {
            return new GroupDto
                {
                    Id = string.IsNullOrWhiteSpace(group.Id) ? 0 : int.Parse(group.Id),
                    Name = group.Name
                };
        }
    }
}