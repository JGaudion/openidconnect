using OpenIDConnect.Core.Domain.Models;

namespace OpenIDConnect.Authorization.Data.UsersApi.Models
{
    public class PageDetailsDto
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int ItemsOnPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public static PageDetailsDto FromDomain(PageDetails paging)
        {
            return new PageDetailsDto
            {
                CurrentPage = paging.CurrentPage,
                PageSize = paging.PageSize,
                ItemsOnPage = paging.ItemsOnPage,
                TotalPages = paging.TotalPages,
                TotalItems = paging.TotalItems
            };
        }

        public PageDetails ToDomain()
        {
            return new PageDetails(this.CurrentPage, this.PageSize, this.ItemsOnPage, this.TotalPages, this.TotalItems);
        }
    }
}
