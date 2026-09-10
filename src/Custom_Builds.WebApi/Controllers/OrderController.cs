using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Enums;
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
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
                
            var result = await orderService.GetPendingOrdersCount(getCurrUserId.Value, cancellationToken);

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
            Guid? currUserId = null;
            if (!User.IsInRole(nameof(RolesEnum.Admin)))
            {
                var getCurrUserId = User.GetId();
                if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();

                currUserId = getCurrUserId.Value;
            }
            
            var result = await orderService.GetDetailsAsync(orderId,currUserId, cancellationToken);

            return result.ToActionResult();
        }

        [Authorize(Roles = nameof(RolesEnum.Admin))]
        [HttpPut("[action]/{orderId:guid}")]
        public async Task<IActionResult> UpdateStatus([FromRoute]Guid orderId, [FromBody]OrderStateEnum newStatus, CancellationToken cancellationToken = default)
        {
            Result result = await orderService.UpdateStatus(orderId, newStatus, cancellationToken);

            return result.ToActionResult();
        }
    }
}
