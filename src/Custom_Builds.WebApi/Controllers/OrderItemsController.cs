using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers;

public class OrderItemsController(
    IOrderItemService orderItemsService
        ) : ApplicationControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderItemDTO>>> GetOrderItems(
        [FromQuery] Guid orderId,
        [FromQuery] LazyDTO lazyData,
        CancellationToken cancellationToken = default)
    {
        Guid? currUserId = null;
        if (!HttpContext.User.IsInRole(nameof(RolesEnum.Admin)))
        {
            var getCurrUserIdResult = User.GetId();
            if (!getCurrUserIdResult.IsSuccess) return ((Result)getCurrUserIdResult).ToActionResult();
            currUserId = getCurrUserIdResult.Value;
        }
        
        var result = await orderItemsService.LazyGetOrderItemsAsync(orderId, currUserId, lazyData, cancellationToken);
        return result.ToActionResult();
    }
}