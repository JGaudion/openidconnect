using System;
using System.Collections.Generic;
using System.Linq;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Extensions
{
    public static class RoleMetadataExtensions
    {
        public static IEnumerable<PropertyMetadata> GetCreateProperties(this RoleMetadata roleMetadata)
        {
            if (roleMetadata == null)
            {
                throw new ArgumentNullException(nameof(roleMetadata));
            }

            var exclude = roleMetadata.CreateProperties.Select(x => x.ClaimType);
            var additional = roleMetadata.UpdateProperties.Where(x => !exclude.Contains(x.ClaimType) && x.Required);

            return roleMetadata.CreateProperties.Union(additional);
        }
    }
}
