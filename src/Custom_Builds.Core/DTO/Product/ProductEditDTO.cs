using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Product
{
    public class ProductEditDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
