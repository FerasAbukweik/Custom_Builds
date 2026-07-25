using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Authorize]
    public class OrderController(
        IOrderService orderService,
        IOrderItemsService orderItemsService
        ) : ApplicationControllerBase
    {
        // add order
        // converts all items in cart to a single order
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result result = await orderService.AddOrderFromCartItemsAsync(getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }

        // get all orders -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<OrderItemDTO>>> GetAllProcessingOrders([FromQuery]LazyDTO lazyData, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await orderItemsService.LazyGetProcessingOrderItemsAsync(getCurrUserId.Value, lazyData, cancellationToken);

            return result.ToActionResult();
        }

        // get all completed orders -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<OrderItemDTO>>> GetAllCompletedOrders([FromQuery]LazyDTO lazyData, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await orderItemsService.LazyGetCompletedOrderItemsAsync(getCurrUserId.Value, lazyData, cancellationToken);

            return result.ToActionResult();
        }

        // get orders count
        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetProcessingOrdersCount(CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await orderItemsService.GetProcessingItemsCountAsync(getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }

        // get AllCompletedOrders count
        [HttpGet("[action]")]
        public async Task<ActionResult<OrderHistoryDTO>> GetHistorySummary(CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await orderService.GetOrderHistoryAsync(getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }

        // buy again
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyAgain([FromBody]Guid orderItemId, CancellationToken  cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result result = await orderItemsService.BuyAgainAsync(getCurrUserId.Value, orderItemId, cancellationToken);

            return result.ToActionResult();
        }
    }
}
