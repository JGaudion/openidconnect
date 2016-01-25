using OpenIDConnect.Users.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OpenIDConnect.Users.Api.Models
{
    public class UserCreateApiModel
    {
        private List<ClaimApiModel> claims;

        [Required]
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }

        public List<ClaimApiModel> Claims
        {
            get
            {
                return this.claims ?? (this.claims = new List<ClaimApiModel>());
            }

            set
            {
                this.claims = value;
            }
        }

        internal User ToDomainModel()
        {
            return new User(
                this.Id, 
                this.Claims.Select(c => new Claim(c.Type, c.Value)));
        }
    }
}