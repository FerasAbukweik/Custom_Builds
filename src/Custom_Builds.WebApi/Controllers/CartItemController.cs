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
        [Transactional]
        public async Task<ActionResult<CartItemDTO>> AddProduct([FromBody] Guid productId, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            // add item to target user cart
            var result = await cartItemService.AddProductAsync(productId,  getCurrUserId.Value, cancellationToken);

            return result.ToActionResult();
        }


        // add custom build
        [HttpPost("[action]")]
        [Transactional]
        public async Task<ActionResult<CartItemDTO>> AddCustomBuild([FromBody] CustomBuildAddDTO toAddCustomBuild, CancellationToken cancellationToken = default)
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            var result = await cartItemService.AddCustomBuildAsync(toAddCustomBuild, getCurrUserId.Value!, cancellationToken);

            return result.ToActionResult();
        }


        // remove cart item
        [HttpDelete("[action]/{toDelCartItemId}")]
        [Transactional]
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
        [Transactional]
        public async Task<IActionResult> UpdateQuantity(IReadOnlyList<Id_Quantity_DTO> needsUpdate, CancellationToken cancellationToken = default)
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
