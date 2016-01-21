using System;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class UserManagementMetadata
    {
        public UserManagementMetadata(UserMetadata userMetadata,
            RoleMetadata roleMetadata)
        {
            if (userMetadata == null)
            {
                throw new ArgumentNullException(nameof(userMetadata));
            }

            if (roleMetadata == null)
            {
                throw new ArgumentNullException(nameof(roleMetadata));
            }

            this.UserMetadata = userMetadata;
            this.RoleMetadata = roleMetadata;
        }

        public UserMetadata UserMetadata { get; }
        public RoleMetadata RoleMetadata { get; }
    }
}
