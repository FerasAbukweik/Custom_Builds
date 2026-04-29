using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IAddCartService _addCartService;
        private readonly IRemoveCartService _removeCartService;
        private readonly IEditCartService _editCartService;
        private readonly IGetCartService _getCartService;

        public CartController(
            IAddCartService addCartService,
            IRemoveCartService removeCartService,
            IEditCartService editCartService,
            IGetCartService getCartService)
        {
            _addCartService = addCartService;
            _removeCartService = removeCartService;
            _editCartService = editCartService;
            _getCartService = getCartService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddCartDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _addCartService.AddAsync(toAdd);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid cartId)
        {
            var result = await _removeCartService.RemoveByIdAsync(cartId);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditCartDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var result = await _editCartService.EditByIdAsync(newData);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Cart>> Get(Guid cartId)
        {
            var result = await _getCartService.GetByIdAsync(cartId);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return Ok(result.Value);
        }
    }
}
