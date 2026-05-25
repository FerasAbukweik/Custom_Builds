using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IOrderServices;
using Custom_Builds.Core.ServiceContracts.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IAddOrderService _addOrderService;
        private readonly IRemoveOrderService _removeOrderService;
        private readonly IGetOrderService _getOrderService;

        public OrderController(
            IAddOrderService addOrderService,
            IRemoveOrderService removeOrderService,
            IGetOrderService getOrderService)
        {
            _addOrderService = addOrderService;
            _removeOrderService = removeOrderService;
            _getOrderService = getOrderService;
        }

        // add order
        [Authorize(Roles = nameof(RoleEnums.User))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] AddOrderDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addOrderService.AddProductAsync(toAdd);

            return result.ToActionResult();
        }

        // remove order
        [Authorize(Roles = nameof(RoleEnums.User))]
        [HttpDelete("[action]/{orderId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid orderId)
        {
            Result result = await _removeOrderService.RemoveByIdAsync(orderId);

            return result.ToActionResult();
        }

        // get all orders -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MiniOrderInfoDTO>>> GetAllProcessingOrders([FromQuery]LazyGetUserDataDTO lazyGetOrdersData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _getOrderService.GetProcessingOrdersAsync(lazyGetOrdersData);

            return result.ToActionResult();
        }

        // get all completed orders -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<List<HistoryOrderDTO>>> GetAllCompletedOrders([FromQuery]LazyGetUserDataDTO lazyGetOrdersData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }


            var result = await _getOrderService.GetCompletedOrdersAsync(lazyGetOrdersData);

            return result.ToActionResult();
        }

        // get orders count
        [HttpGet("[action]")]
        public async Task<ActionResult<int>> GetProcessingOrdersCount([FromQuery] Guid? userId)
        {
            var result = await _getOrderService.GetProcessingOrdersCountAsync(userId);

            return result.ToActionResult();
        }

        // get AllCompletedOrders count
        [HttpGet("[action]")]
        public async Task<ActionResult<OrderHistoryDTO>> GetHistorySummary([FromQuery] Guid? userId)
        {
            var result = await _getOrderService.GetHistorySummaryAsync(userId);

            return result.ToActionResult();
        }

        // buy again
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyAgain([FromBody]Guid orderId)
        {
            Result result = await _addOrderService.BuyAgain(orderId);

            return result.ToActionResult();
        }
    }
}
