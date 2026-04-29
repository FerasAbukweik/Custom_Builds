using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class EditCartService : IEditCartService
    {
        private readonly ICartRepository _cartRepository;

        public EditCartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<Result> EditByIdAsync(EditCartDTO newData)
        {
            return _cartRepository.EditByIdAsync(newData);
        }
    }
}
