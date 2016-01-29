namespace OpenIDConnect.Core.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PagingApiModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
    
        [Range(1, 50)]
        public int PageSize { get; set; }
    }
}