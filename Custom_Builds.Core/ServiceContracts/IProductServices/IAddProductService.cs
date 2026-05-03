using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IProductServices
{
    public interface IAddProductService
    {
        Task<Result<ProductDTO>> AddAsync(AddProductDTO toAdd);
    }
}
