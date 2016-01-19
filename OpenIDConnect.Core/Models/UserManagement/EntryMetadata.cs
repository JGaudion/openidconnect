using System.Collections.Generic;
using System.Linq;

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
            if (updateProperties != null)
            {
                this.UpdateProperties = updateProperties;
            }
            if (createProperties != null)
            {
                this.CreateProperties = createProperties;
            }
        }

        public bool SupportsCreate { get; }
        public bool SupportsDelete { get; }
        public IEnumerable<PropertyMetadata> UpdateProperties { get; } = Enumerable.Empty<PropertyMetadata>();
        public IEnumerable<PropertyMetadata> CreateProperties { get; } = Enumerable.Empty<PropertyMetadata>();
    }
}
