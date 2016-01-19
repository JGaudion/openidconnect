using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class UserSummary
    {
        public UserSummary(string subject, string username, string name)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Subject = subject;
            this.Username = username;
            this.Name = name;
        }

        public string Subject { get; }

        public string Username { get; }

        public string Name { get; }
    }
}
