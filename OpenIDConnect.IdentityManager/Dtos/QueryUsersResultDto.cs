using System.Collections.Generic;

namespace OpenIDConnect.IdentityManager.Dtos
{
    class QueryUsersResultDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public int Total { get; set; }

        public IEnumerable<UserDto> Items { get; set; }
    }
}
