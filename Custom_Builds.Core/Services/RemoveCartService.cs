using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RemoveCartService : IRemoveCartService
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<Result> RemoveByIdAsync(Guid cartId)
        {
            return _cartRepository.RemoveByIdAsync(cartId);
        }
    }
}
