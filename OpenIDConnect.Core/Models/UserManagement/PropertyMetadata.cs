using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class PropertyMetadata
    {
        public PropertyMetadata(
            string propertyType,
            string claimType,
            string name,
            bool required)
        {
            if (string.IsNullOrWhiteSpace(propertyType))
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.PropertyType = propertyType;
            this.ClaimType = claimType;
            this.Name = name;
            this.Required = required;
        }

        public string PropertyType { get; }
        public string ClaimType { get; }
        public string Name { get; }
        public bool Required { get; }
    }
}