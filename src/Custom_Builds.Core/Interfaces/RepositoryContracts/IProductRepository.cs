using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
        void Add(Product toAdd);
        Task<Product?> EditByIdAsync(ProductEditDTO newData, CancellationToken cancellationToken = default);
        Task<Product?> RemoveByIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> FilterAsync(
            Expression<Func<Product, bool>> extraChecks,
            Expression<Func<Product, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> LazyGetAllProductsAsync(
            LazyDTO reqData,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MiniInventoryItemDTO>> GetDashboardMiniInfoAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<Product, bool>> filters, CancellationToken cancellationToken = default);
    }
}
