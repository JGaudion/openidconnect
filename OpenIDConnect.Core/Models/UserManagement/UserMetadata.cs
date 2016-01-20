using System.Collections.Generic;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class UserMetadata : EntryMetadata
    {
        public UserMetadata(
            bool supportsCreate,
            bool supportsDelete,
            bool supportsClaims,
            IEnumerable<PropertyMetadata> updateProperties,
            IEnumerable<PropertyMetadata> createProperties)
            : base(supportsCreate, supportsDelete, updateProperties, createProperties)
        {
            this.SupportsClaims = supportsClaims;
        }

        public bool SupportsClaims { get; }
    }
}
