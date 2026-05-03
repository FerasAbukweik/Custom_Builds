using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class GetCartItemService : IGetCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public GetCartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Result<List<MiniCartItemDTO>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData)
        {
            var result = await _cartItemRepository.GetAllCartItemsAsync(getData);
            if (!result.IsSuccess) return result.MapFailure<List<MiniCartItemDTO>>();

            return Result<List<MiniCartItemDTO>>.Success(result.Value!);
        }

        public async Task<Result<CartItem>> GetByIdAsync(Guid cartItemId)
        {
            var result = await _cartItemRepository.GetByIdAsync(cartItemId);
            if (!result.IsSuccess) return result.MapFailure<CartItem>();

            return Result<CartItem>.Success(result.Value!);
        }
    }
}
