using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Part
{
    public class PartAddDTO
    {
        [Required]
        public required string Icon { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
