using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class ProductService(
    IProductRepository productRepository,
    ILogger<ProductService> logger) : IProductService
{
    public async Task<Result<ProductDTO>> AddAsync(ProductAddDTO_DB toAdd, CancellationToken cancellationToken = default)
    {
        // new product
        Product newProduct = new Product()
        {
            Id = Guid.NewGuid(),
            Title = toAdd.Name,
            Price = toAdd.Price,
            Description = toAdd.Description,
            Images = toAdd.Images
        };

        productRepository.Add(newProduct);

        if (!await productRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ProductService), nameof(AddAsync));
            return Result<ProductDTO>.Failure("failed saving changes to DB");
        }

        return Result<ProductDTO>.Success(newProduct.toDTO());
    }
    public async Task<Result<IReadOnlyList<ProductDTO>>> LazyGetAllAsync(LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var result = await productRepository.LazyGetAllProductsAsync(lazyData, cancellationToken);

        return Result<IReadOnlyList<ProductDTO>>.Success(result.Select(p => p.toDTO()).ToList());
    }
    public async Task<Result<ProductDTO>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var result = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (result == null)
        {
            logger.LogWarning("{serviceName}.{methodName} failed to get product with id: {productId}",
                nameof(ProductService), nameof(GetByIdAsync), productId);
            return Result<ProductDTO>.Failure("product not found");
        }

        return Result<ProductDTO>.Success(result.toDTO());
    }
    public async Task<Result<ProductDTO>> RemoveByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var removed = await productRepository.RemoveByIdAsync(productId, cancellationToken);
        if (removed == null)
        {
            logger.LogWarning("{serviceName}.{methodName} failed to get product with id: {productId}",
                nameof(ProductService), nameof(RemoveByIdAsync), productId);
            return Result<ProductDTO>.Failure("product not found");
        }
        
        if (!await productRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ProductService), nameof(RemoveByIdAsync));
            return Result<ProductDTO>.Failure("failed saving changes to DB");
        }

        return Result<ProductDTO>.Success(removed.toDTO());
    }
}