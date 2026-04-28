using Custom_Builds.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class AddItemDTO
    {
        [Required(ErrorMessage = "{0} Is Reqiered")]
        public required string Name { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Reqiered")]
        [Range(minimum:0 , maximum: 100000000 , ErrorMessage = "Invalid {0} Price Should Be Between {1} and {2}")]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid FieldId { get; set; }
    }
}
