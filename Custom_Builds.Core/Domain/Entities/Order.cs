using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public OrderStateEnum OrderStatus { get; set; } = OrderStateEnum.Pending;

        [Required(ErrorMessage = "{0} is required.")]
        public required OrderTypeEnum OrderType { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public required string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "{0} is required")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "{0} is required")]
        public required decimal TotalPrice { get; set; }


        public Guid? CustomBuildId { get; set; }
        public CustomBuild? CustomBuild { get; set; }

        [MustHaveOneOnly(nameof(CustomBuildId), ErrorMessage = "must have only one of {0} or {1}")]
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }


        public OrderDTO toDTO()
        {
            return new OrderDTO
            {
                Id = this.Id,
                OrderType = this.OrderType,
                Title = this.Title,
                CreatedAt = this.CreatedAt,
                CustomBuildId = this.CustomBuildId,
                OrderStatus = this.OrderStatus,
                ProductId = this.ProductId,
                TotalPrice = this.TotalPrice,
                Quantity = this.Quantity
            };
        }
    }
}
