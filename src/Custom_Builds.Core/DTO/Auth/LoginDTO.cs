using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.Constants;

namespace Custom_Builds.Core.DTO.Auth
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PasswordRegex, ErrorMessage = "Invalid Password")]
        public required string Password { get; set; }
    }
}
