using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddCartItemDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required OrderTypeEnum orderType { get; set; }



        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid ProductId { get; set; }
    }
}
