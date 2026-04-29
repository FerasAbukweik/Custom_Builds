using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddOrderDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(typeof(decimal), "0", "100000000", ErrorMessage = "Invalid {0}. TotalPrice should be between {1} and {2}.")]
        public required decimal TotalPrice { get; set; }
    }
}
