using System.Collections.Generic;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class RoleMetadata : EntryMetadata
    {
        public RoleMetadata(
            bool supportsCreate,
            bool supportsDelete,
            string roleClaimType,
            IEnumerable<PropertyMetadata> updateProperties,
            IEnumerable<PropertyMetadata> createProperties)
            : base(supportsCreate, supportsDelete, updateProperties, createProperties)
        {
            this.RoleClaimType = roleClaimType;
        }

        public string RoleClaimType { get; }
    }
}
