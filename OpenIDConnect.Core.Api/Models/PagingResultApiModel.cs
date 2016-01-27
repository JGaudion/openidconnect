using System.Collections.Generic;

namespace OpenIDConnect.Core.Api.Models
{
    public class PagingResultApiModel<TItem>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public int Total { get; set; }

        public IEnumerable<TItem> Items { get; set; }
    }
}
