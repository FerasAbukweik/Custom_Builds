using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO
{
    public class AddCartItemToDB_DTO
    {
        public required decimal TotalPrice { get; set; }
        public required OrderTypeEnum orderType { get; set; }
        public required Guid UserId { get; set; }
        public Guid? CustomBuildId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
