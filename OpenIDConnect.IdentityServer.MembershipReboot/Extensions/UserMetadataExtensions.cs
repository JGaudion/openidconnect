using System;
using System.Collections.Generic;
using System.Linq;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Extensions
{
    public static class UserMetadataExtensions
    {
        public static IEnumerable<PropertyMetadata> GetCreateProperties(this UserMetadata userMetadata)
        {
            if (userMetadata == null)
            {
                throw new ArgumentNullException(nameof(userMetadata));
            }

            var exclude = userMetadata.CreateProperties.Select(x => x.ClaimType);
            var additional = userMetadata.UpdateProperties.Where(x => !exclude.Contains(x.ClaimType) && x.Required);

            return userMetadata.CreateProperties.Union(additional);
        }
    }
}
