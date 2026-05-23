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

        [Required(ErrorMessage = "{0} is required.")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "{0} is required.")]
        public required DateTime AddedAt { get; set; }

        public CartItemDTO toDTO()
        {
            return new CartItemDTO()
            {
                Id = this.Id,
                orderType = this.orderType,
                CustomBuildId = this.CustomBuildId,
                ProductId = this.ProductId,
                Price = this.orderType switch
                {
                    OrderTypeEnum.Product => this.Product?.Price ?? -1,
                    OrderTypeEnum.Custom => throw new Exception("Custom Build Price Must Be Passed to the toDTO() fun"),
                    _ => throw new Exception("unknown order type")
                },
                image = this.orderType switch
                {
                    OrderTypeEnum.Product => this.Product?.images.FirstOrDefault() ?? "no image",
                    OrderTypeEnum.Custom => "Custom Build image",
                    _ => throw new Exception("unknown order type")
                },
                Quantity = this.Quantity,
                Title = this.orderType switch
                {
                    OrderTypeEnum.Product => this.Product?.Name ?? "missing product",
                    OrderTypeEnum.Custom => "Custom Build",
                    _ => throw new Exception("unknown order type")
                },
                Specs = ["Product"]
            };
        }

        public CartItemDTO toDTO(decimal totalPrice)
        {
            return new CartItemDTO()
            {
                Id = this.Id,
                orderType = this.orderType,
                CustomBuildId = this.CustomBuildId,
                ProductId = this.ProductId,
                Price = totalPrice,
                image = this.orderType switch
                {
                    OrderTypeEnum.Product => this.Product?.images.FirstOrDefault() ?? "no image",
                    OrderTypeEnum.Custom => "Custom Build image",
                    _ => throw new Exception("unknown order type")
                },
                Title = this.orderType switch
                {
                    OrderTypeEnum.Product => this.Product?.Name ?? "missing product",
                    OrderTypeEnum.Custom => "Custom Build",
                    _ => throw new Exception("unknown order type")
                },
                Quantity = this.Quantity,
                Specs = ["Custom Build"]
            };
        }
    }
}
