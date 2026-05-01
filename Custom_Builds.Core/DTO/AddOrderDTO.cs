using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class AddOrderDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid ProductId { get; set; }
    }
}
