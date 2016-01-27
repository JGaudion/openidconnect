namespace OpenIDConnect.Authorization.Data.EntityFramework.Dtos
{
    using OpenIDConnect.Authorization.Domain.Models;

    public class GroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClientId { get; set; }

        public ClientDto Client { get; set; }

        public Group ToDomainModel()
        {
            return new Group(this.Id.ToString(), this.Name);
        }
    }
}