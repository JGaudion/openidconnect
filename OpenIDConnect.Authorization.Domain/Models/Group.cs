using System;

namespace OpenIDConnect.Authorization.Domain.Models
{
    public class Group
    {        
        public Group(string id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Id = id;
            this.Name = name;
        }

        public string Id { get; }

        public string Name { get; }
    }
}