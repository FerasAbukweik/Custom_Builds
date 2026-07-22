using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Lazy
{
    public class LazyDTO
    {
        [Required]
        public required int Taken { get; set; }

        [Required]
        public required int SectionSize { get; set; }
    }
}
