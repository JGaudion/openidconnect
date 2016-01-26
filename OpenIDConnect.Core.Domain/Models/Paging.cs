namespace OpenIDConnect.Core.Domain.Models
{
    public class Paging
    {
        public Paging(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; }

        public int PageSize { get; }
    }
}
