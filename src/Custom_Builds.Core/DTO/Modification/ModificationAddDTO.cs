using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Modification
{
    public class ModificationAddDTO
    {
        [Required]
        public required string Name { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        
        [Required]
        public required string Type { get; set; }
        public string? Icon { get; set; }       

        [Required]
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. Price should be between {1} and {2}.")]
        public required decimal Price { get; set; }

        [Required]
        public required Guid SectionId { get; set; }
    }
}
