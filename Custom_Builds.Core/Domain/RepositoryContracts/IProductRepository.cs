using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IProductRepository
    {
        Task<Result<Product>> GetByIdAsync(Guid productId);
        Task<Result<Product>> AddAsync(Product toAdd);
        Task<Result> EditByIdAsync(EditProductDTO newData);
        Task<Result> RemoveByIdAsync(Guid productId);
        Task<Result<List<Product>>> FilterAsync(Expression<Func<Product, bool>> extraChecks, Expression<Func<Product, object>>[]? includes = null);
    }
}
