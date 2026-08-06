using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.ValidationAttributes;

namespace Custom_Builds.Core.Domain.Entities;

public class OrderItem
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public required OrderTypeEnum OrderType { get; set; }
    
    [Required]
    public required int Quantity { get; set; }
    
    [Required]
    public required decimal OrderedPrice { get; set; }
    
    
    
    // relations
    
    [Required]
    public required Guid OrderId { get; set; }
    public Order? Order { get; set; }
        
    [MustHaveOneOnly(nameof(ProductId))]
    public Guid? CustomBuildId { get; set; }
    public CustomBuild? CustomBuild { get; set; }

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    
    
    
    // DTO
    // must include product, customBuild and Order based on order type
    public OrderItemDTO ToDTO()
    {
        return new OrderItemDTO()
        {
            Id = Id,
            OrderType = OrderType,
            Quantity = Quantity,
            OrderedPrice = OrderedPrice,
            State = Order?.OrderStatus ?? OrderStateEnum.Processing,
            Image = OrderType switch
            {
                OrderTypeEnum.Custom => "Custom Build",
                OrderTypeEnum.Product => Product?.Images[0] ?? "unknown",
                _ => throw new Exception("unhandled order type")
            },
            Title = OrderType switch
            {
                OrderTypeEnum.Custom => "Custom Build",
                OrderTypeEnum.Product => Product?.Title ?? "unknown",
                _ => throw new Exception("unhandled order type")
            },
            Specs = OrderType switch
            {
                OrderTypeEnum.Product => ["Product"],
                OrderTypeEnum.Custom => CustomBuild?.Modifications.Select(m => m.Name).ToList() ?? ["Custom Build"],
                _ => throw new Exception("unhandled order type")
            }
        };
    }
}