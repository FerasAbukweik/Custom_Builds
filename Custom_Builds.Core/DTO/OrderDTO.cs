using Custom_Builds.Core.CustomValidationAttributes;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public required decimal TotalPrice { get; set; }
        public OrderStateEnum OrderStatus { get; set; } = OrderStateEnum.Pending;
        public required OrderTypeEnum OrderType { get; set; }
        public required string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
