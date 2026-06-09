using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;
using System.Net;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class GetCartItemService : IGetCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly IGetCustomBuildService _getCustomBuildService;

        public GetCartItemService(ICartItemRepository cartItemRepository,
                                  IGetCurrUserService getCurrUserService,
                                  IGetCustomBuildService getCustomBuildService)
        {
            _cartItemRepository = cartItemRepository;
            _getCurrUserService = getCurrUserService;
            _getCustomBuildService = getCustomBuildService;
        }

        public async Task<Result<List<CartItemDTO>>> LazyGetAllCartItemsAsync(LazyGetUserDataDTO getData)
        {
            // get target userId to insure
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(getData.UserId);
            if (!getTargetUserIdResult.IsSuccess) getTargetUserIdResult.MapFailure<CartItemDTO>();

            // add target user id to the request
            getData.UserId = getTargetUserIdResult.Value!;

            // get target user cart items -- with include product so we can access product price  
            var result = await _cartItemRepository.LazyGetCartItems(getData);
            if (!result.IsSuccess) return result.MapFailure<List<CartItemDTO>>();


            List<CartItemDTO> newCartItems = new List<CartItemDTO>();

            foreach (var item in result.Value!)
            {
                // if its a normal product just take its price
                if (item.orderType == OrderTypeEnum.Product) newCartItems.Add(item.toDTO());
                

                // if it is custom build get its price first
                else if (item.orderType == OrderTypeEnum.Custom)
                {
                    var getCustomBuildPriceResult = await _getCustomBuildService.GetPriceAsync(item.CustomBuildId!.Value);
                    if (!getCustomBuildPriceResult.IsSuccess) throw new Exception(getCustomBuildPriceResult.ErrorMessage);

                    newCartItems.Add(item.toDTO(getCustomBuildPriceResult.Value!));
                }

                else
                {
                    throw new Exception("unhandled order type at GetAllCartItemsAsync");
                }
            }

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
        public async Task<Result<CartSummaryDTO>> GetCurrUserSummaryAsync()
        {
            var getCurrUserResult = _getCurrUserService.GetUserId();
            if (!getCurrUserResult.IsSuccess) return getCurrUserResult.MapFailure<CartSummaryDTO>();

            var getSummaryResult = await _cartItemRepository.GetSummaryInfoAsync(getCurrUserResult.Value!);

            return getSummaryResult;
        }
    }
}
