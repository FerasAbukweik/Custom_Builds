using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly IAddCartItemService _addCartItemService;
        private readonly IRemoveCartItemService _removeCartItemService;
        private readonly IGetCartItemService _getCartItemService;

        public CartItemController(
            IAddCartItemService addCartItemService,
            IRemoveCartItemService removeCartItemService,
            IGetCartItemService getCartItemService)
        {
            _addCartItemService = addCartItemService;
            _removeCartItemService = removeCartItemService;
            _getCartItemService = getCartItemService;
        }



        // add normal product
        [Authorize(Roles = ($"{nameof(RoleEnums.User)}"))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] Guid productId)
        {
            // add item to target user cart
            Result result = await _addCartItemService.AddAsync(productId);

            return result.ToActionResult();
        }


        // add custom build
        [Authorize(Roles = ($"{nameof(RoleEnums.User)}"))]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCustomBuild([FromBody] AddCustomBuildDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            Result result = await _addCartItemService.AddCustomBuildAsync(toAdd);

            return result.ToActionResult();
        }


        // remove cart item
        [Authorize(Roles = ($"{nameof(RoleEnums.User)}"))]
        [HttpDelete("[action]/{toDelCartItemId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid toDelCartItemId)
        {
            Result result = await _removeCartItemService.RemoveByIdAsync(toDelCartItemId);

            return result.ToActionResult();
        }


        // get cart items -- with lazy loading
        [Authorize(Roles = ($"{nameof(RoleEnums.User)} , {nameof(RoleEnums.Admin)}"))]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<CartItemDTO>>> GetCartItems([FromQuery]LazyGetCartItemsDTO getData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }
            
            var result = await _getCartItemService.GetAllCartItemsAsync(getData);

            return result.ToActionResult();
        }
    }
}
