
using System;

namespace OpenIDConnect.Users.Domain.Models
{
    public class Claim
    {
        private readonly string type;

        private readonly string value;

        public Claim(string type, string value)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.type = type;
            this.value = value;
        }

        public string Type
        {
            get
            {
                return this.type;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
        }
    }
}