using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class UserDetail : UserSummary
    {
        public UserDetail(string subject, string username, string name, IEnumerable<Claim> claims, IEnumerable<Claim> properties)
            : base(subject, username, name)
        {
            if (claims != null)
            {
                this.Claims = claims;
            }

            if (properties != null)
            {
                this.Properties = properties;
            }
        }

        public IEnumerable<Claim> Claims { get; } = Enumerable.Empty<Claim>();

        public IEnumerable<Claim> Properties { get; } = Enumerable.Empty<Claim>();
    }
}
