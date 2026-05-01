using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class RemoveProductService : IRemoveProductService
    {
        private readonly IProductRepository _productRepository;
        public RemoveProductService(IProductRepository productRepository) => _productRepository = productRepository;
        public Task<Result> RemoveByIdAsync(Guid productId) => _productRepository.RemoveByIdAsync(productId);
    }
}
