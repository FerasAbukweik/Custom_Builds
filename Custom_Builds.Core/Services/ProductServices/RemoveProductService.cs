using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class RemoveProductService : IRemoveProductService
    {
        private readonly IProductRepository _productRepository;
        public RemoveProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid productId)
        {
            var result = await _productRepository.RemoveByIdAsync(productId);
            if (!result.IsSuccess) return result.MapFailure();

            return Result.Success();
        }
    }
}
