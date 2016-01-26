using OpenIDConnect.Users.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenIDConnect.Users.Api.Models
{
    public class ClaimApiModel
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        internal Claim ToDomainModel()
        {
            return new Claim(this.Type, this.Value);
        }
    }
}