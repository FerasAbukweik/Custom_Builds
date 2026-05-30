using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;
        public GetProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<List<ProductDTO>>> GetAllAsync(LazyDTO reqData)
        {
            var result = await _productRepository.LazyGetAllProductsAsync(reqData);
            if (!result.IsSuccess) return result.MapFailure<List<ProductDTO>>();

            return Result<List<ProductDTO>>.Success(result.Value!.Select(p => p.toDTO()).ToList());

        }
        public async Task<Result<Product>> GetByIdAsync(Guid productId)
        {
            var result = await _productRepository.GetByIdAsync(productId);
            if (!result.IsSuccess) return result.MapFailure<Product>();

            return Result<Product>.Success(result.Value!);
        }
        Task<Result<ProductDTO>> IGetProductService.GetByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }
    }
}
