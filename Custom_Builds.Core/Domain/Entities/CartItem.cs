using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required OrderTypeEnum orderType { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? CustomBuildId { get; set; }
        public CustomBuild? CustomBuild { get; set; }

        [MustHaveOneOnly(nameof(CustomBuildId), ErrorMessage = "must have only one of {0} or {1}")]
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        public CartItemDTO toDTO()
        {
            return new CartItemDTO()
            {
                Id = this.Id,
                CustomBuildId = this.CustomBuildId,
                orderType = this.orderType,
                ProductId = this.ProductId,
                TotalPriced = this.Product!.Price
            };
        }

        public CartItemDTO toDTO(decimal totalPrice)
        {
            return new CartItemDTO()
            {
                Id = this.Id,
                CustomBuildId = this.CustomBuildId,
                orderType = this.orderType,
                ProductId = this.ProductId,
                TotalPriced = totalPrice
            };
        }
    }
}
