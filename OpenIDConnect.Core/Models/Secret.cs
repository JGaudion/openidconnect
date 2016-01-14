using System;

namespace OpenIDConnect.Core.Models
{
    public class Secret
    {
        public Secret(string value,
            string description = default(string),
            string type = default(string),
            DateTimeOffset? expiration = default(DateTimeOffset?))
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
            this.Description = description;
            this.Type = type;
            this.Expiration = expiration;
        }

        public string Value { get; }
        public string Description { get; }
        public DateTimeOffset? Expiration { get; }
        public string Type { get; }
    }
}
