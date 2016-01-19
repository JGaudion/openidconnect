using System;

namespace OpenIDConnect.AdLds.Models
{
    public class AdLdsUser
    {
        public AdLdsUser(
            string id,
            string displayName,
            string userName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
            this.DisplayName = displayName;
            this.UserName = userName;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public string UserName { get; }

        public string Email { get; set; }
    }
}
