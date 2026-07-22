namespace Custom_Builds.Core.DTO.Cart
{
    public class CartSummaryDTO
    {
        public required decimal TotalPrice { get; set; }
        public required int TotalOrders { get; set; }
        public decimal Tax { get; set; } = 0;
        public decimal ShippingCost { get; set; } = 0;
    }
}
