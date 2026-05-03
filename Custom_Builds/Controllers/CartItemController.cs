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
        private readonly IEditCartItemService _editCartItemService;
        private readonly IGetCartItemService _getCartItemService;
        private readonly IGetCurrUserService _getCurrUserService;

        public CartItemController(
            IAddCartItemService addCartItemService,
            IRemoveCartItemService removeCartItemService,
            IEditCartItemService editCartItemService,
            IGetCartItemService getCartItemService,
            IGetCurrUserService getCurrUserService)
        {
            _addCartItemService = addCartItemService;
            _removeCartItemService = removeCartItemService;
            _editCartItemService = editCartItemService;
            _getCartItemService = getCartItemService;
            _getCurrUserService = getCurrUserService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(AddCartItemDTO toAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            // get target userId
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(toAdd.UserId);
            if (!getTargetUserIdResult.IsSuccess)
            {
                return ((Result)getTargetUserIdResult).ToActionResult();
            }

            // add target user id to the request
            toAdd.UserId = getTargetUserIdResult.Value!;


            // add item to target user cart
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

            // get target userId
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(toAdd.CreatorId);
            if (!getTargetUserIdResult.IsSuccess)
            {
                return ((Result)getTargetUserIdResult).ToActionResult();
            }

            // add target user id to the request
            toAdd.CreatorId = getTargetUserIdResult.Value!;

            Result result = await _addCartItemService.AddCustomBuildAsync(toAdd);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = nameof(RoleEnums.Admin))]
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
            var result = await _getCartItemService.GetByIdAsync(cartItemId);

            return result.ToActionResult();
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MiniCartItemDTO>>> GetCartItems(LazyGetCartItemsDTO getData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.CollectErrors());
            }

            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(getData.UserId);
            if (!getTargetUserIdResult.IsSuccess)
            {
                // convert to Result so its guranteed we return ActionResult and not ActionResult<Data>
                return ((Result)getTargetUserIdResult).ToActionResult();
            }

            getData.UserId = getTargetUserIdResult.Value!;
            
            var result = await _getCartItemService.GetAllCartItemsAsync(getData);

            return result.ToActionResult();
        }
    }
}
