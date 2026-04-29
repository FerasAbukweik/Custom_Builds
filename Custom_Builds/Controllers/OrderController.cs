using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.ServiceContracts;
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

        public OrderController(
            IAddOrderService addOrderService,
            IRemoveOrderService removeOrderService,
            IEditOrderService editOrderService,
            IGetOrderService getOrderService)
        {
            _addOrderService = addOrderService;
            _removeOrderService = removeOrderService;
            _editOrderService = editOrderService;
            _getOrderService = getOrderService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddOrderDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _addOrderService.AddAsync(toAdd);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid orderId)
        {
            var result = await _removeOrderService.RemoveByIdAsync(orderId);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditOrderDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _editOrderService.EditByIdAsync(newData);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Order>> Get(Guid orderId)
        {
            var result = await _getOrderService.GetByIdAsync(orderId);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }
    }
}
