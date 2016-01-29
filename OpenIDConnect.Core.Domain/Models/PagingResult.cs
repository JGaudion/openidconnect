using System.Collections.Generic;

namespace OpenIDConnect.Core.Domain.Models
{
    using System;
    using System.Linq;

    public class PagingResult<TItem>
    {
        private IEnumerable<TItem> items;

        public PagingResult(PageDetails pageDetails, IEnumerable<TItem> items)
        {
            if (pageDetails == null)
            {
                throw new ArgumentNullException(nameof(pageDetails));
            }

            this.Paging = pageDetails;
            this.items = items;
        }

        public PageDetails Paging { get; }

        public IEnumerable<TItem> Items => this.items ?? (this.items = Enumerable.Empty<TItem>());
    }
}