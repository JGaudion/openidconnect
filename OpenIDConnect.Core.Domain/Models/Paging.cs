namespace OpenIDConnect.Core.Domain.Models
{
    using System;

    public class Paging
    {
        public Paging(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new ArgumentException("Page value is invalid", nameof(page));
            }

            if (pageSize < 1 || pageSize > 50)
            {
                throw new ArgumentException("Page size is invalid", nameof(pageSize));
            }

            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; }

        public int PageSize { get; }
    }
}