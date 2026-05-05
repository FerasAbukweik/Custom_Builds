using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditCustomBuildDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? NewModifications { get; set; }
        public CustomBuildTypeEnum? NewCustomBuildType { get; set; }
    }
}
