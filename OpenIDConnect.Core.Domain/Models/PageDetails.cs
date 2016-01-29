namespace OpenIDConnect.Core.Domain.Models
{
    public class PageDetails
    {        
        public PageDetails(int currentPage, int pageSize, int itemsOnPage, int totalPages, int totalItems)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.ItemsOnPage = itemsOnPage;
            this.TotalPages = totalPages;
            this.TotalItems = totalItems;
        }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public int ItemsOnPage { get; }

        public int TotalPages { get; }

        public int TotalItems { get; }
    }
}