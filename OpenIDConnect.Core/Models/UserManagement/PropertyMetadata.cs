using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class PropertyMetadata
    {
        public PropertyMetadata(string type,
            string name,
            bool required)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Type = type;
            this.Name = name;
            this.Required = required;
        }

        public string Type { get; }
        public string Name { get; }
        public bool Required { get; }
    }
}