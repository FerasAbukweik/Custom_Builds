using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Section
{
    public class SectionAddDTO
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required Guid PartId { get; set; }
    }
}
