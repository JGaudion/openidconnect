using System;

namespace OpenIDConnect.Users.Domain.Models
{
    public class User
    {
        private readonly string id;

        public User(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.id = id;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
