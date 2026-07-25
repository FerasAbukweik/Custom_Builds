using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.DTO.Lazy;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface ICartItemRepository
    {
        void Add(CartItem toAdd);
        Task<CartItem?> GetByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default);
        Task<CartItem?> RemoveByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default);
        Task<List<CartItem>> FilterAsync(Expression<Func<CartItem, bool>> extraChecks,
            Expression<Func<CartItem, object?>>[]? includes = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);
        void UpdateRange(List<CartItem> newItems);
        Task<CartSummaryDTO?> GetSummaryInfoAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CartItem>> UpdateQuantitiesAsync(IReadOnlyList<Id_Quantity_DTO> needsUpdate,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
