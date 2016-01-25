using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Users.Domain.Models
{
    public class User
    {        
        private readonly string id;

        private IEnumerable<Claim> claims;

        public User(string id, IEnumerable<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }            

            this.id = id;
            this.claims = claims;
        }

        public string Id
        {
            get
            {
                return this.id;
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