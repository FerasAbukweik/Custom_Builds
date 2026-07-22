using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Custom_Builds.Core.DTO.Product
{
    public class ProductAddDTO
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        [Range(typeof(decimal), "0", "100000000")]
        public required decimal Price { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        [Required]
        public required List<IFormFile> Images { get; set; }
    }
}
