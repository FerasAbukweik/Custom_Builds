using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.Cart
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public required OrderTypeEnum orderType { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
        public required Decimal Price { get; set; }
        public required int Quantity { get; set; }
        public required string Title { get; set; }
        public List<string> Specs { get; set; } = new List<string>();
        public required string image { get; set; }
    }
}