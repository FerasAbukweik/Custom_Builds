using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IProductServices
{
    public interface IGetProductService
    {
        Task<Result<Product>> GetByIdAsync(Guid productId);
    }
}
