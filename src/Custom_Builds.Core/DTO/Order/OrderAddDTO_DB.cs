using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.Order
{
    public class OrderAddDTO_DB
    {
        public required Guid UserId { get; set; }
        public required decimal TotalPrice { get; set; }
        public required OrderTypeEnum OrderType { get; set; }
        public required string Title { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
