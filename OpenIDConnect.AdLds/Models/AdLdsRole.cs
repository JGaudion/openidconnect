using System;

namespace OpenIDConnect.AdLds.Models
{
    public class AdLdsRole
    {
        public AdLdsRole(
            string subject,
            string name,
            string description)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            this.Subject = subject;
            this.Name = name;
            this.Description = description;
        }

        public string Subject { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
