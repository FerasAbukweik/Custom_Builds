using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class RemoveCartItemService : IRemoveCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public RemoveCartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid cartItemId)
        {
            return await _cartItemRepository.RemoveByIdAsync(cartItemId);
        }
    }
}
