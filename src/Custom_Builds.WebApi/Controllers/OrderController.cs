using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Authorize]
    public class OrderController(
        IOrderService orderService
        ) : ApplicationControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrders([FromQuery]LazyDTO lazyData, CancellationToken cancellationToken = default)
        {
            var getCurrUserId = User.GetId();
            if(!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await orderService.LazyGetOrdersAsync(getCurrUserId.Value, lazyData, cancellationToken);

            return result.ToActionResult();
        }
        
        // add order
        // converts all items in cart to a single order
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result result = await orderService.AddOrderWithCartItemsAsync(getCurrUserId.Value, cancellationToken);

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


        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetPendingOrdersCount(CancellationToken cancellationToken = default)
        {
            var result = await orderService.GetPendingOrdersCount(cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetPendingOrders([FromQuery]LazyDTO lazyData, CancellationToken cancellationToken = default)
        {
            var getCurrUserId = User.GetId();
            if(!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();

            var result = await orderService.LazyGetPendingOrdersAsync(getCurrUserId.Value, lazyData, cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails([FromQuery]Guid orderId, CancellationToken cancellationToken = default)
        {
            var result = await orderService.GetDetailsAsync(orderId, cancellationToken);

            return result.ToActionResult();
        }
    }
}
