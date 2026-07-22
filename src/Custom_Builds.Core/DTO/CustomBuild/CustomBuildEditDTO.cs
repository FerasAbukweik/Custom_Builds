using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.CustomBuild
{
    public class CustomBuildEditDTO
    {
        [Required]
        public required Guid Id { get; set; }
        public string? NewModifications { get; set; }
        public CustomBuildTypeEnum? NewCustomBuildType { get; set; }
    }
}
