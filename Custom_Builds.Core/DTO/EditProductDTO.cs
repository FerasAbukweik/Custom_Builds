using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditProductDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
