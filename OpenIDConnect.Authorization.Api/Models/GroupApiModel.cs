namespace OpenIDConnect.Authorization.Api.Models
{
    using OpenIDConnect.Authorization.Domain.Models;

    public class GroupApiModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public GroupApiModel()
        {            
        }

        public GroupApiModel(Group group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
        }

        public Group ToDomainModel()
        {
            return new Group(this.Id, this.Name);
        }
    }
}