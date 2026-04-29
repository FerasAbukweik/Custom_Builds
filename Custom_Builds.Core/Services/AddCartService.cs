using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class AddCartService : IAddCartService
    {
        private readonly ICartRepository _cartRepository;

        public AddCartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<Result> AddAsync(AddCartDTO toAdd)
        {
            return _cartRepository.AddAsync(toAdd);
        }
    }
}
