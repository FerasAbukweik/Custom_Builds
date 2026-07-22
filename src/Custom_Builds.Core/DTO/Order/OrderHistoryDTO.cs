namespace Custom_Builds.Core.DTO.Order
{
    public class OrderHistoryDTO
    {
        public required decimal TotalPrice { get; set; }
        public required int Count { get; set; }
    }
}