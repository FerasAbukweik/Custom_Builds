using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddCartItemDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required OrderTypeEnum orderType { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public Guid? UserId { get; set; }


        public Guid? ProductId { get; set; }

        [MustHaveOneOnly(nameof(ProductId))]
        public Guid? CustomBuildId { get; set; }
    }
}
