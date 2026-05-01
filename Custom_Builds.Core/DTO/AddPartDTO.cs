using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.DTO
{
    public class AddPartDTO
    {
        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Name { get; set; }
    }
}
