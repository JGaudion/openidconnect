using System.Collections.Generic;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public abstract class EntryMetadata
    {
        protected EntryMetadata(
            bool supportsCreate, 
            bool supportsDelete, 
            IEnumerable<PropertyMetadata> updateProperties,
            IEnumerable<PropertyMetadata> createProperties)
        {
            this.SupportsCreate = supportsCreate;
            this.SupportsDelete = supportsDelete;
        }

        public bool SupportsCreate { get; }
        public bool SupportsDelete { get; }
        public IEnumerable<PropertyMetadata> UpdateProperties { get; }
        public IEnumerable<PropertyMetadata> CreateProperties { get; }
    }
}
