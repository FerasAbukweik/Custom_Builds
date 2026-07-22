using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO.Order
{
    public class OrderAddDTO
    {
        [Required]
        public required Guid UserId { get; set; }

        [Required]
        public required Guid ProductId { get; set; }

        [Required]
        public required int Quantity { get; set; }
    }
}
