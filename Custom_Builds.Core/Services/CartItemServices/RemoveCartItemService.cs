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
            var getCurrUserId = _getCurrUserService.GetUserId();
            if (!getCurrUserId.IsSuccess) return getCurrUserId;

            var getCartItemResult = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (!getCartItemResult.IsSuccess) return getCartItemResult;

            if (getCartItemResult.Value!.Id == getCurrUserId.Value!)
            {
                var removeCartItemResult = await _cartItemRepository.RemoveAsync(getCartItemResult.Value!);
                if (!removeCartItemResult.IsSuccess) return removeCartItemResult;

                return Result.Success();
            }
            else
            {
                return Result.Failure("User Is Unauthorized", HttpStatusCode.Unauthorized);
            }
        }
    }
}
