using System;
using System.DirectoryServices;

namespace OpenIDConnect.IdentityServer3.AdLds
{
    public class AdLdsUser
    {
        public AdLdsUser(string id,
            string name,
            string email)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Id = id;
            this.Name = name;
            this.Email = email;
        }

        public string Id { get; }

        public string Name { get; }

        public string Email { get; }
    }
}
