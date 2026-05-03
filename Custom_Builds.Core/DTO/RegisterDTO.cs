using Custom_Builds.Core.Constants;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "{0} Is requiered")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} Is requiered")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} Is requiered")]
        [RegularExpression(ValidationConstants.PhoneNumberRegex , ErrorMessage ="Wrong Phone Number Format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} Is requiered")]
        [RegularExpression(ValidationConstants.PasswordRegex , ErrorMessage = "Wrong Password Format")]
        public string Password { get; set; } = string.Empty;
        public RoleEnums role { get; set; } = RoleEnums.User;
    }
}
