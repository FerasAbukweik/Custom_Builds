using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using System.Net;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class RemoveCartItemService : IRemoveCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetCurrUserService _getCurrUserService;

        public RemoveCartItemService(ICartItemRepository cartItemRepository,
                                     IGetCurrUserService getCurrUserService)
        {
            _cartItemRepository = cartItemRepository;
            _getCurrUserService = getCurrUserService;
        }

        public async Task<Result> RemoveByIdAsync(Guid cartItemId)
        {
            // is current logged in user an admin ? 
            var isAdminResult = _getCurrUserService.IsAdmin();
            if (!isAdminResult.IsSuccess) return isAdminResult;

            // if not admin make sure that cart item is his
            if (!isAdminResult.Value)
            {
                // get cart item so we can check its owner and delete it fast next time we need _cartItemRepository.Remove
                var getCartItemResult = await _cartItemRepository.GetByIdAsync(cartItemId);
                if (!getCartItemResult.IsSuccess) return getCartItemResult;

                // get current logged in user id so we can compare it with cart item owner id
                var getUserIdResult = _getCurrUserService.GetUserId();
                if (!getUserIdResult.IsSuccess) return getUserIdResult;

                // check if user is not admin and not owner of the cart item return unauthorized
                if (getCartItemResult.Value!.UserId != getUserIdResult.Value)
                {
                    return Result.Failure("You are not authorized to remove this cart item." , HttpStatusCode.Unauthorized);
                }

                // remove cart item if current user is owner of it
                await _cartItemRepository.RemoveAsync(getCartItemResult.Value!);
                return Result.Success();
            }

            // if admin remove cart item directly without checking its owner
            await _cartItemRepository.RemoveByIdAsync(cartItemId);

            return Result.Success();
        }
    }
}
