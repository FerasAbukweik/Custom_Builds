using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
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

        public async Task<Result<CartItem>> GetItemByIdAsync(Guid cartItemId)
        {
            return await _cartItemRepository.GetByIdAsync(cartItemId);
        }
    }
}
