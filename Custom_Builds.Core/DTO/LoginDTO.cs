using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "{0} is reqiered")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "{0} is reqiered")]
        public required string Password { get; set; }
    }
}
