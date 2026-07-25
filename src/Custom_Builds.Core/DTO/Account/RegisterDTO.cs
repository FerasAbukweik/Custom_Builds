using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.Constants;

namespace Custom_Builds.Core.DTO.Account
{
    public class RegisterDTO
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PhoneNumberRegex , ErrorMessage ="Wrong Phone Number Format")]
        public required string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PasswordRegex , ErrorMessage = "Wrong Password Format")]
        public required string Password { get; set; }
    }
}
