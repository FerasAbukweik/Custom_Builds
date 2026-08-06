using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.Order
{
    public class OrderDTO
    {
        public required Guid Id { get; set; }
        public required decimal OrderedPrice { get; set; }
        public required OrderStateEnum OrderStatus { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
