using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ICartItemRepository
    {
        Task<Result<CartItem>> GetByIdAsync(Guid cartItemId);
        Task<Result<CartItem>> AddAsync(AddCartItemToDB_DTO toAdd);
        Task<Result> EditByIdAsync(EditCartItemDTO newData);
        Task<Result> RemoveByIdAsync(Guid cartItemId);
        Task<Result> RemoveAsync(CartItem toDel);
        Task<Result<List<MiniCartItemDTO>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData);
    }
}
