using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Modification
{
    public class ModificationLinkDTO
    {
        [Required]
        public required Guid sectionId { get; set; }

        [Required]
        public required Guid modificationId { get; set; }
    }
}
