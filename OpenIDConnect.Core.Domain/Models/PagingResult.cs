using System.Collections.Generic;

namespace OpenIDConnect.Core.Domain.Models
{
    public class PagingResult<TItem>
    {
        public PagingResult(int page, int pageSize, int count, int total, IEnumerable<TItem> items)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.Count = count;
            this.Total = total;
            this.Items = items;
        }

        public int Page { get; }

        public int PageSize { get; }

        public int Count { get; }

        public int Total { get; }

        public IEnumerable<TItem> Items { get; }
    }
}
