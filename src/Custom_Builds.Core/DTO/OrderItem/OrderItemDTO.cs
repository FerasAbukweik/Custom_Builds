using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.OrderItem;

public class OrderItemDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required OrderTypeEnum OrderType { get; set; }
    public required int Quantity { get; set; }
    public required string Title { get; set; }
    public required decimal OrderedPrice { get; set; }
    public required string Image { get; set; }
    public required IReadOnlyList<string> Specs = [];
    public required OrderStateEnum State { get; set; }
}