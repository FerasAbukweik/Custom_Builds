using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Lazy
{
    public class LazyDTO
    {
        [Required]
        [Range(0, int.MaxValue)]
        public required int Taken { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public required int SectionSize { get; set; }
    }
}