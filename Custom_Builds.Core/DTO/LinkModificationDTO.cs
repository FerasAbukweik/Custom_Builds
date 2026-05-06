using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class LinkModificationDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        public required Guid sectionId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public required Guid modificationId { get; set; }
    }
}
