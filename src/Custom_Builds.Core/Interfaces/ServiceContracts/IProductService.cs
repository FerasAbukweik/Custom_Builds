using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IProductService
{
    Task<Result<ProductDTO>> AddAsync(ProductAddDTO_DB toAdd, CancellationToken cancellationToken = default);
    Task<Result<List<ProductDTO>>> GetAllAsync(LazyDTO reqData, CancellationToken cancellationToken = default);
    Task<Result<ProductDTO>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<Result<ProductDTO>> RemoveByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}