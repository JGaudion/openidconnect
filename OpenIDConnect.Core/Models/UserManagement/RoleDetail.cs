using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class RoleDetail : RoleSummary
    {
        public RoleDetail(string subject, string name, string description, IEnumerable<Claim> claims)
            : base(subject, name, description)
        {
            if (claims != null)
            {
                this.Claims = claims;
            }
        }

        public IEnumerable<Claim> Claims { get; } = Enumerable.Empty<Claim>();
    }
}
