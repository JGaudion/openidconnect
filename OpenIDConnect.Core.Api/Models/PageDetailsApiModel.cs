namespace OpenIDConnect.Core.Api.Models
{
    using OpenIDConnect.Core.Domain.Models;

    public class PageDetailsApiModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int ItemsOnPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public static PageDetailsApiModel FromDomain(PageDetails paging)
        {
            return new PageDetailsApiModel
                {
                    CurrentPage = paging.CurrentPage,
                    PageSize = paging.PageSize,
                    ItemsOnPage = paging.ItemsOnPage,
                    TotalPages = paging.TotalPages,
                    TotalItems = paging.TotalItems
                };
        }
    }
}