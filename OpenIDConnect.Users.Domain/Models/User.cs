using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Users.Domain.Models
{
    public class User
    {        
        private readonly string id;

        private readonly string username;

        private IEnumerable<Claim> claims;

        public User(string id, string username, IEnumerable<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }            

            this.id = id;
            this.username = username;
            this.claims = claims;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public string Username
        {
            get
            {
                return this.username;
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                return this.claims ?? (this.claims = Enumerable.Empty<Claim>());
            }

            set
            {
                this.claims = value;
            }
        }
    }
}