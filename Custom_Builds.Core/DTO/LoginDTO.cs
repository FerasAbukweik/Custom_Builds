using Custom_Builds.Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "{0} is reqiered")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "{0} is reqiered")]
        [RegularExpression(ValidationConstants.PasswordRegex, ErrorMessage = "Invalid Password")]
        public required string Password { get; set; }
    }
}
