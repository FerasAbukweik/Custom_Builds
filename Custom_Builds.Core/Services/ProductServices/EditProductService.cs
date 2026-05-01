using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.ProductServices
{
    public class EditProductService : IEditProductService
    {
        private readonly IProductRepository _productRepository;
        public EditProductService(IProductRepository productRepository) => _productRepository = productRepository;
        public Task<Result> EditByIdAsync(EditProductDTO newData) => _productRepository.EditByIdAsync(newData);
    }
}
