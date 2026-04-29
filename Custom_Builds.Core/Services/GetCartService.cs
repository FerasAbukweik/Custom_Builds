using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class GetCartService : IGetCartService
    {
        private readonly ICartRepository _cartRepository;

        public GetCartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<Result<Cart>> GetByIdAsync(Guid cartId)
        {
            return _cartRepository.GetByIdAsync(cartId);
        }
    }
}
