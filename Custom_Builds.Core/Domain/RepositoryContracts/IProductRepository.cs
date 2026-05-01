using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IProductRepository
    {
        Task<Result<Product>> GetByIdAsync(Guid productId);
        Task<Result<Guid>> AddAsync(AddProductDTO toAdd);
        Task<Result> EditByIdAsync(EditProductDTO newData);
        Task<Result> RemoveByIdAsync(Guid productId);
    }
}
