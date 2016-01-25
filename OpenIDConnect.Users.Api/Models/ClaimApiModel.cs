using System.ComponentModel.DataAnnotations;

namespace OpenIDConnect.Users.Api.Models
{
    public class ClaimApiModel
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}