using System;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddItemDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required string Name { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }       

        [Required(ErrorMessage = "{0} is required.")]
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. Price should be between {1} and {2}.")]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid FieldId { get; set; }
    }
}
