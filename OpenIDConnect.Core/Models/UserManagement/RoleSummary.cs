using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class RoleSummary
    {
        public RoleSummary(string subject,
            string name,
            string description)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description));
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
