using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class EditCartItemService : IEditCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public EditCartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Result> EditByIdAsync(EditCartItemDTO newData)
        {
            return await _cartItemRepository.EditByIdAsync(newData);
        }
    }
}
