using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddProductDTO
    {
        [Required]
        public required string Name { get; set; }
        [Range(typeof(decimal), "0", "100000000")]
        public required decimal Price { get; set; }
    }
}
