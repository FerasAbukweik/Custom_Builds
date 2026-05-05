using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class GetCartItemService : IGetCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly IGetProductService _getProductService;

        public GetCartItemService(ICartItemRepository cartItemRepository,
                                  IGetCurrUserService getCurrUserService,
                                  IGetProductService getProductService)
        {
            _cartItemRepository = cartItemRepository;
            _getCurrUserService = getCurrUserService;
            _getProductService = getProductService;
        }

        public async Task<Result<List<CartItemDTO>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData)
        {
            // get target userId to insure
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(getData.UserId);
            if (!getTargetUserIdResult.IsSuccess) getTargetUserIdResult.MapFailure<CartItemDTO>();

            // add target user id to the request
            getData.UserId = getTargetUserIdResult.Value!;

            // get target user cart items -- with include product so we can access product price  
            var result = await _cartItemRepository.FilterAsync(ci => ci.UserId == getData.UserId, [ci => ci.Product!]);
            if (!result.IsSuccess) return result.MapFailure<List<CartItemDTO>>();

            List<CartItemDTO> newCartItems = result.Value!.Select(c => c.toDTO()).ToList();

            return Result<List<CartItemDTO>>.Success(newCartItems);
        }
        public async Task<Result<CartItemDTO>> GetByIdAsync(Guid cartItemId)
        {
            // get target user id
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(cartItemId);
            if (!getTargetUserIdResult.IsSuccess) getTargetUserIdResult.MapFailure<CartItemDTO>();

            // get cart item
            var getCartItemResult = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (!getCartItemResult.IsSuccess) return getCartItemResult.MapFailure<CartItemDTO>();

            // check if target user is the owner of the item
            if(getCartItemResult.Value!.UserId == getTargetUserIdResult.Value!)
            {
                return Result<CartItemDTO>.Success(getCartItemResult.Value!.toDTO());
            }
            else
            {
                return Result<CartItemDTO>.Failure("Target user isnt the owner of the item");
            }
        }
    }
}
