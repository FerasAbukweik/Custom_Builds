namespace Custom_Builds.Core.DTO.Order;

public class OrderManagementStatusDto
{
    public required int PendingOrdersCount { get; set; }
    public required int LatestOrdersCount { get; set; }
}