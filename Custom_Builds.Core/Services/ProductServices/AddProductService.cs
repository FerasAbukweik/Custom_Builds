using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class AddProductService : IAddProductService
    {
        private readonly IProductRepository _productRepository;
        public AddProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result<ProductDTO>> AddAsync(AddProductDTO toAdd)
        {
            var result = await _productRepository.AddAsync(toAdd);
            if (!result.IsSuccess) return result.MapFailure<ProductDTO>();

            return Result<ProductDTO>.Success(result.Value!.toDTO());
        }
    }
}
