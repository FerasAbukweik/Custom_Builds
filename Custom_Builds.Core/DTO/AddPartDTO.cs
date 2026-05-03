using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddPartDTO
    {
        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Name { get; set; }
    }
}
