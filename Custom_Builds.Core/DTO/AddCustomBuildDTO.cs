using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddCustomBuildDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }

        public List<Guid> ModificationIds { get; set; } = new List<Guid>();

        [Required(ErrorMessage = "{0} is requiered")]
        public CustomBuildTypeEnum CustomBuildType { get; set; }
    }
}
