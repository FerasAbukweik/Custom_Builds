using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Custom_Builds.Core.DTO.Order;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public OrderStateEnum OrderStatus { get; set; } = OrderStateEnum.Processing;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        // relations
        
        [Required]
        public required Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public List<OrderItem> OrderedItems = [];
        
        
        // DTO
        // must include OrderedItems
        public OrderDTO toDTO()
        {
            return new OrderDTO
            {
                Id = Id,
                CreatedAt = CreatedAt,
                OrderStatus = OrderStatus,
                OrderedPrice = OrderedItems.Sum(o => (o.OrderedPrice * o.Quantity)),
            };
        }

        // to order details dto
        // must include user
        public OrderDetailsDto ToDetailsDto()
        {
            return new OrderDetailsDto()
            {
                Id = Id,
                OrderedDate = CreatedAt,
                PhoneNumber = User?.PhoneNumber ?? "unknown",
                UserName = User?.UserName ?? "unknown",
                UserId = UserId,
                Status = OrderStatus
            };
        }
    }
}
