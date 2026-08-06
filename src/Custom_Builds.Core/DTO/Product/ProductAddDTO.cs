using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Product
{
    public class ProductAddDTO
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required decimal Price { get; set; }
                
        [Required]
        public required string Description { get; set; }
                
        [Required]
        public required List<string> Images { get; set; }
                
        [Required]
        public required int InStock { get; set; }
    }
}
