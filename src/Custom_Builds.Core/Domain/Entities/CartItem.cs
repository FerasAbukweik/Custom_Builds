using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.ValidationAttributes;

namespace Custom_Builds.Core.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required OrderTypeEnum OrderType { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public required decimal OrderPrice { get; set; }
        
        
        
        // relations
        
        [Required]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        
        
        [MustHaveOneOnly(nameof(ProductId))]
        public Guid? CustomBuildId { get; set; }
        public CustomBuild? CustomBuild { get; set; }
        
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        
        
        // DTO
        
        // convert cartItem object to dto
        // requires including product or customBuild based on item type 
        public CartItemDTO ToDTO()
        {
            return new CartItemDTO()
            {
                Id = Id,
                orderType = OrderType,
                CustomBuildId = CustomBuildId,
                ProductId = ProductId,
                Price = OrderPrice,
                Quantity = Quantity,
                image = OrderType switch
                {
                    OrderTypeEnum.Product => Product?.Images.FirstOrDefault() ?? "no image",
                    OrderTypeEnum.Custom => "Custom Build image",
                    _ => throw new Exception("unknown order type")
                },
                Title = OrderType switch
                {
                    OrderTypeEnum.Product => Product?.Title ?? "missing product",
                    OrderTypeEnum.Custom => "Custom Build",
                    _ => throw new Exception("unhandled order type")
                },
                Specs = CustomBuild?.Modifications.Select(m =>m.Name).ToList() ?? ["Custom Build"]
            };
        }
    }
}
