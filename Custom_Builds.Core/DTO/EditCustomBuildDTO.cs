using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditCustomBuildDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? SelectedModificationIds { get; set; }
        public CustomBuildTypeEnum? CustomBuildType { get; set; }
    }
}
