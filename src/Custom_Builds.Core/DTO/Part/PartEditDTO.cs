using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Part
{
    public class PartEditDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? Icon { get; set; }
        public string? Name { get; set; }
    }
}
