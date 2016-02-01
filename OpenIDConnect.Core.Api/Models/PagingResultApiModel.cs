using System.Collections.Generic;

namespace OpenIDConnect.Core.Api.Models
{
    public class PagingResultApiModel<TItem>
    {
        private IEnumerable<TItem> items;        

        public PageDetailsApiModel Paging { get; set; }

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
