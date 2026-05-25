using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IProductServices
{
    public interface IGetProductService
    {
        Task<Result<ProductDTO>> GetByIdAsync(Guid productId);
        Task<Result<List<ProductDTO>>> GetAllAsync(LazyDTO reqData);
    }
}
