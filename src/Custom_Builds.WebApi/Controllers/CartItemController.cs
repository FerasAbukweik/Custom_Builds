using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.DTO.CustomBuild;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{ 
    [Authorize]
    public class CartItemController(
        ICartItemService cartItemService
        ) : ApplicationControllerBase
    {
        // add normal product
        [HttpPost("[action]")]
        public async Task<IActionResult> AddProduct([FromBody] Guid productId, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            // add item to target user cart
            Result result = await cartItemService.AddProductAsync(productId,  getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }


        // add custom build
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCustomBuild([FromBody] CustomBuildAddDTO toCustomBuildAdd, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result result = await cartItemService.AddCustomBuildAsync(toCustomBuildAdd, getCurrUserId.Value!, cancellationToken);

            return result.ToActionResult();
        }


        // remove cart item
        [HttpDelete("[action]/{toDelCartItemId}")]
        public async Task<IActionResult> Remove([FromRoute]Guid toDelCartItemId, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result result = await cartItemService.RemoveByIdAsync(toDelCartItemId, getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }


        // get cart items -- with lazy loading
        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<CartItemDTO>>> GetCartItems([FromQuery] LazyDTO lazyData, CancellationToken cancellationToken  = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await cartItemService.LazyGetAllCartItemsAsync(lazyData, getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }


        // update quantities
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateQuantity(IReadOnlyList<Id_Quantity_DTO_ts> needsUpdate, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            Result updateResult = await cartItemService.UpdateQuantitiesAsync(needsUpdate, getCurrUserId.Value, cancellationToken);

            return updateResult.ToActionResult();
        }

        // get summary info
        [HttpGet("[action]")]
        public async Task<ActionResult<CartSummaryDTO>> GetSummaryInfo(CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await cartItemService.GetSummaryAsync(getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }
    }
}
