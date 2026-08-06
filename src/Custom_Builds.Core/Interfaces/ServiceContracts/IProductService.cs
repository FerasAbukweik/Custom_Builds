using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IProductService
{
    Task<Result<ProductDTO>> AddAsync(ProductAddDTO toAdd, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProductDTO>>> LazyGetAllAsync(LazyDTO lazyData, CancellationToken cancellationToken = default);
    Task<Result<ProductDTO>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<Result<ProductDTO>> RemoveByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<MiniInventoryItemDTO>>> GetDashboardMiniInfoAsync(int take, CancellationToken cancellationToken = default);
    Task<Result<int>> GetLowStockCountAsync(int lowAmount, CancellationToken cancellationToken = default);
}