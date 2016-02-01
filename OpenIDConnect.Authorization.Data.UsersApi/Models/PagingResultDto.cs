using System.Collections.Generic;

namespace OpenIDConnect.Authorization.Data.UsersApi.Models
{
    public class PagingResultDto<TItem>
    {
        private IEnumerable<TItem> items;

        public PageDetailsDto Paging { get; set; }

        public IEnumerable<TItem> Items
        {
            get
            {
                return this.items ?? (this.items = new List<TItem>());
            }

            set
            {
                this.items = value;
            }
        }
    }
}
