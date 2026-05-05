using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICartItemServices
{
    public interface IGetCartItemService
    {
        Task<Result<CartItemDTO>> GetByIdAsync(Guid cartItemId);
        Task<Result<List<CartItemDTO>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData);
    }
}
