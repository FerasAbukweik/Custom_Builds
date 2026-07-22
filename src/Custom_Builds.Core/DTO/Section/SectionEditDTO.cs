using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Section
{
    public class SectionEditDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? Title { get; set; }
    }
}
