using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditSectionDTO
    {
        [Required(ErrorMessage = "{0} Is requiered")]
        public required Guid Id { get; set; }
        public string? Title { get; set; }
    }
}
