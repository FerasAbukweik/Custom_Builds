using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class LazyDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required int Section { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required int ElementsPerSection { get; set; }
    }
}
