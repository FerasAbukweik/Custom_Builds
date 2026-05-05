using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ICartItemRepository
    {
        Task<Result<CartItem>> GetByIdAsync(Guid cartItemId);
        Task<Result<CartItem>> AddAsync(CartItem toAdd);
        Task<Result> RemoveByIdAsync(Guid cartItemId);
        Task<Result> RemoveAsync(CartItem toDel);
        Task<Result<List<CartItem>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData);
        Task<Result<List<CartItem>>> FilterAsync(Expression<Func<CartItem, bool>> extraChecks, Expression<Func<CartItem, object>>[]? includes = null);
    }
}
