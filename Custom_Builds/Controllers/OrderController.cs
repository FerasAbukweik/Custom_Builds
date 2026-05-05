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
        private readonly IEditOrderService _editOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IGetCurrUserService _getCurrUserService;

        public OrderController(
            IAddOrderService addOrderService,
            IRemoveOrderService removeOrderService,
            IEditOrderService editOrderService,
            IGetOrderService getOrderService,
            IGetCurrUserService getCurrUserService)
        {
            _addOrderService = addOrderService;
            _removeOrderService = removeOrderService;
            _editOrderService = editOrderService;
            _getOrderService = getOrderService;
            _getCurrUserService = getCurrUserService;
        }

        // add cart item
        [Authorize(Roles = nameof(RoleEnums.User))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] AddOrderDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addOrderService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        // remove cart item
        [Authorize(Roles = nameof(RoleEnums.User))]
        [HttpDelete("[action]/{orderId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid orderId)
        {
            Result result = await _removeOrderService.RemoveByIdAsync(orderId);

            return result.ToActionResult();
        }

        // get all cart items -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MiniOrderInfoDTO>>> GetAll([FromQuery]LazyGetALlOrdersDTO lazyGetOrdersData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _getOrderService.GetUserOrdersAsync(lazyGetOrdersData);

            return result.ToActionResult();
        }

        // get all cart completed items -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MiniOrderInfoDTO>>> GetAllCompletedOrders([FromQuery]LazyGetALlOrdersDTO lazyGetOrdersData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }


            var result = await _getOrderService.GetCompletedUserOrdersAsync(lazyGetOrdersData);

            return result.ToActionResult();
        }
    }
}
