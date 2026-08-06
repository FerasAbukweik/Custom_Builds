using Custom_Builds.Core.DTO.Product;

namespace Custom_Builds.Core.DTO.Admin;

public class DashboardDTO
{
    public required decimal TotalRevenue { get; set; }
    public required int PendingOrdersCount { get; set; }
    public required int LowStockAlerts { get; set; }

    public required IReadOnlyList<decimal> WeeklyRevenue { get; set; }
    public required IReadOnlyList<decimal> MonthlyRevenue { get; set; }
    
    public required IReadOnlyList<MiniInventoryItemDTO> InventoryItems { get; set; }
}