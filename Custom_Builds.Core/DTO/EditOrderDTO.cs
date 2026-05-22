using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditOrderDTO
    {
        [Required(ErrorMessage = "{0} is required.")]
        public required Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Title { get; set; }
        public int? Quantity { get; set; }
        public OrderStateEnum? OrderStatus { get; set; }
    }
}
