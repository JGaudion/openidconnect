using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class PropertyMetadata
    {
        public PropertyMetadata(
            string displayFieldType,
            string claimType,
            string name,
            bool required)
        {
            if (string.IsNullOrWhiteSpace(displayFieldType))
            {
                throw new ArgumentNullException(nameof(displayFieldType));
            }

            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.DisplayFieldType = displayFieldType;
            this.ClaimType = claimType;
            this.Name = name;
            this.Required = required;
        }

        public string DisplayFieldType { get; }
        public string ClaimType { get; }
        public string Name { get; }
        public bool Required { get; }
    }
}