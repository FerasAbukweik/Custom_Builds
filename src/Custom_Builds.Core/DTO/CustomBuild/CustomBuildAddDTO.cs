using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.CustomBuild
{
    public class CustomBuildAddDTO
    {

        [Required]
        public required IReadOnlyList<Guid> ModificationIds { get; set; }

        [Required]
        public required CustomBuildTypeEnum CustomBuildType { get; set; }
    }
}
