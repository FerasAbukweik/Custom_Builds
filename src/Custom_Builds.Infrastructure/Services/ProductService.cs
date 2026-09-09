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
    ILogger<ProductService> logger,
    IImageService imageService) : IProductService
{
    public async Task<Result<ProductDTO>> AddAsync(ProductAddDTO toAdd, CancellationToken cancellationToken = default)
    {
        // new product
        Product newProduct = new Product()
        {
            Title = toAdd.Name,
            Price = toAdd.Price,
            Description = toAdd.Description,
            InStock = toAdd.InStock
        };
        productRepository.Add(newProduct);
        

        // add images and link them with new product + save changes
        var addImagesResult = await imageService.AddRange(toAdd.Images.ToArray(), newProduct.Id, cancellationToken);
        if(!addImagesResult.IsSuccess) return addImagesResult.MapFailure<ProductDTO>();

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

    public async Task<Result<IReadOnlyList<MiniInventoryItemDTO>>> GetDashboardMiniInfoAsync(int take, CancellationToken cancellationToken = default)
    {
        return Result<IReadOnlyList<MiniInventoryItemDTO>>.Success(await productRepository.GetDashboardMiniInfoAsync(null ,take, cancellationToken));
    }

    public async Task<Result<int>> GetLowStockCountAsync(int lowAmount ,CancellationToken cancellationToken = default)
    {
        return Result<int>.Success(await productRepository.CountAsync(p => p.InStock <= lowAmount, cancellationToken));
    }

    public async Task<Result<ProductDTO>> EditAsync(ProductEditDTO editData, CancellationToken cancellationToken = default)
    {
        var edited = await productRepository.EditByIdAsync(editData, cancellationToken);
        
        if(edited == null) return Result<ProductDTO>.Failure("failed to edit product");
        
        if (!await productRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ProductService), nameof(RemoveByIdAsync));
            return Result<ProductDTO>.Failure("failed saving changes to DB");
        }
        
        return Result<ProductDTO>.Success(edited.toDTO());
    }
}