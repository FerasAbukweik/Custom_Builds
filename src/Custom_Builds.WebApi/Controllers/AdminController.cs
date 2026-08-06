using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Admin;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers;

// [Authorize(Roles = nameof(RolesEnum.Admin))] disabled for testing
public class AdminController(
    IProductService productService,
    IOrderService orderService
    ) : ApplicationControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<DashboardDTO>> GetDashboardData(CancellationToken cancellationToken = default)
    {
        var getInventoryItemsResult = await productService.GetDashboardMiniInfoAsync(10, cancellationToken);
        if (!getInventoryItemsResult.IsSuccess) return ((Result)getInventoryItemsResult).ToActionResult();
        
        var getLowStockResult = await productService.GetLowStockCountAsync(10, cancellationToken);
        if(!getLowStockResult.IsSuccess) return ((Result)getLowStockResult).ToActionResult();
        
        var getMonthlyRevenueResult = await orderService.GetDailyRevenueAsync(30, cancellationToken);
        if(!getMonthlyRevenueResult.IsSuccess) return ((Result)getMonthlyRevenueResult).ToActionResult();
        
        var getWeeklyRevenueResult = await orderService.GetDailyRevenueAsync(7, cancellationToken);
        if(!getMonthlyRevenueResult.IsSuccess) return ((Result)getMonthlyRevenueResult).ToActionResult();

        var getPendingOrdersCountResult = await orderService.GetPendingOrdersCount(cancellationToken);
        if(!getPendingOrdersCountResult.IsSuccess) return ((Result)getPendingOrdersCountResult).ToActionResult();

        var getTotalRevenueResult = await orderService.GetTotalRevenueAsync(cancellationToken);
        if(!getTotalRevenueResult.IsSuccess) return ((Result)getTotalRevenueResult).ToActionResult();

        var result = new DashboardDTO()
        {
            InventoryItems = getInventoryItemsResult.Value!,
            LowStockAlerts = getLowStockResult.Value!,
            MonthlyRevenue = getMonthlyRevenueResult.Value!,
            WeeklyRevenue = getWeeklyRevenueResult.Value!,
            PendingOrdersCount = getPendingOrdersCountResult.Value!,
            TotalRevenue = getTotalRevenueResult.Value!
        };

        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<OrderManagementStatusDto>> GetOrderManagementStatus(CancellationToken cancellationToken = default)
    {
        var getPendingOrdersCount = await orderService.GetPendingOrdersCount(cancellationToken);
        if (!getPendingOrdersCount.IsSuccess) return ((Result)getPendingOrdersCount).ToActionResult();

        var getLatestOrdersCountResult = await orderService.GetLatestOrdersCountAsync(1, cancellationToken);
        if (!getLatestOrdersCountResult.IsSuccess) return ((Result)getLatestOrdersCountResult).ToActionResult();

        var result = new OrderManagementStatusDto
        {
            PendingOrdersCount = getPendingOrdersCount.Value,
            LatestOrdersCount = getLatestOrdersCountResult.Value
        };

        return Ok(result);
    }
    
    [HttpGet("[action]")]
    public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetPendingOrders([FromQuery]LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var result = await orderService.LazyGetPendingOrdersAsync(null, lazyData, cancellationToken);

        return result.ToActionResult();
    }
}