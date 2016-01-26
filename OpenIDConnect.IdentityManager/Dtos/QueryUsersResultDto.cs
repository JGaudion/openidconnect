using System.Collections.Generic;

namespace OpenIDConnect.IdentityManager.Dtos
{
    class QueryUsersResultDto
    {
        public int Start { get; set; }

        public int Count { get; set; }

        public int Total { get; set; }

        public string Filter { get; set; }

        public IEnumerable<UserDto> Users { get; set; }
    }
}
