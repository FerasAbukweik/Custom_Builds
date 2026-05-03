using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddSectionDTO
    {
        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid PartId { get; set; }
    }
}
