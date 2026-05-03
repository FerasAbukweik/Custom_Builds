using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO
{
    public class MiniOrderInfoDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public string Image { get; set; } = "No image";
        public required OrderStateEnum status { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}