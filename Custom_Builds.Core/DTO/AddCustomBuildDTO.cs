using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddCustomBuildDTO
    {

        [Required(ErrorMessage = "{0} is requiered")]
        public required List<Guid> ModificationIds { get; set; }

        [Required(ErrorMessage = "{0} is requiered")]
        public CustomBuildTypeEnum CustomBuildType { get; set; }
    }
}
