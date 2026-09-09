using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Custom_Builds.Core.DTO.Modification
{
    public class ModificationAddDTO
    {
        [Required]
        public required string Name { get; set; }
        
        [Required]
        public required IFormFile Image { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. Price should be between {1} and {2}.")]
        public required decimal Price { get; set; }

        [Required]
        public required Guid SectionId { get; set; }
    }
}
