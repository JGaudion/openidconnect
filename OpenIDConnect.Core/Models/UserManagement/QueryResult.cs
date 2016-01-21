using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class QueryResult<TResult>
    {
        public QueryResult(string filter)
            : this(0, 0, 0, filter, Enumerable.Empty<TResult>())
        {
        }

        public QueryResult(int start, int count, int total, string filter, IEnumerable<TResult> items)
        {
            this.Start = start;
            this.Count = count;
            this.Total = total;
            this.Filter = filter;
            this.Items = items;
        }

        public int Start { get; }
        public int Count { get; }
        public int Total { get; }
        public string Filter { get; }
        public IEnumerable<TResult> Items { get; }
    }
}
