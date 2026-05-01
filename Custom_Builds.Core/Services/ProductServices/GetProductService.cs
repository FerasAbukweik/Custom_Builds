using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;
        public GetProductService(IProductRepository productRepository) => _productRepository = productRepository;
        public Task<Result<Product>> GetByIdAsync(Guid productId) => _productRepository.GetByIdAsync(productId);
    }
}
