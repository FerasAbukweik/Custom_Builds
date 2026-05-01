using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IAddCartItemService _addCartItemService;
        private readonly IRemoveCartItemService _removeCartItemService;
        private readonly IEditCartItemService _editCartItemService;
        private readonly IGetCartItemService _getCartItemService;

        public CartItemController(
            IAddCartItemService addCartItemService,
            IRemoveCartItemService removeCartItemService,
            IEditCartItemService editCartItemService,
            IGetCartItemService getCartItemService)
        {
            _addCartItemService = addCartItemService;
            _removeCartItemService = removeCartItemService;
            _editCartItemService = editCartItemService;
            _getCartItemService = getCartItemService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddCartItemDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }



            Result result = await _addCartItemService.AddAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCustomBuild(AddCustomBuildDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }



            Result result = await _addCartItemService.AddCustomBuildAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Remove(Guid cartItemId)
        {
            Result result = await _removeCartItemService.RemoveByIdAsync(cartItemId);

            return result.ToActionResult();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(EditCartItemDTO newData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _editCartItemService.EditByIdAsync(newData);

            return result.ToActionResult();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<CartItem>> GetItem(Guid cartItemId)
        {
            var result = await _getCartItemService.GetItemByIdAsync(cartItemId);

            return result.ToActionResult();
        }
    }
}
