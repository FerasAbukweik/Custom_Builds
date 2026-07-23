using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.DTO.CustomBuild;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class CartItemService(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    ICustomBuildService customBuildService,
    ILogger<CartItemService> logger,
    IModificationsService modificationsService
    ) : ICartItemService
{
    public async Task<Result<CartItemDTO>> AddProductAsync(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // get product
        var product = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
            return Result<CartItemDTO>.Failure("Product not found");
        
        // new item to add
        CartItem newCartItem = new CartItem()
        {
            Id = Guid.NewGuid(),
            OrderType = OrderTypeEnum.Product,
            UserId = userId,
            ProductId = productId,
            OrderPrice = product.Price
        };

        // adding item to the cart
        cartItemRepository.Add(newCartItem);

        if (!await cartItemRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} Failed saving changes toDB",
                nameof(CartItemService), nameof(AddProductAsync));
            return Result<CartItemDTO>.Failure("Failed saving changes to DB");
        }

        return Result<CartItemDTO>.Success(newCartItem.ToDTO());
    }
    
    public async Task<Result<CartItemDTO>> AddCustomBuildAsync(
        CustomBuildAddDTO toAddCustomBuild,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // make new custom build based on List<Modification> in the customBuild table so we can link it with cart item
        var addCustomBuildResult = await customBuildService.AddCustomBuild(toAddCustomBuild,userId, cancellationToken);
        if (!addCustomBuildResult.IsSuccess)
            return addCustomBuildResult.MapFailure<CartItemDTO>();
        
        // get modifications price
        var getPriceResult =
            await modificationsService.GetModificationsPriceAsync(toAddCustomBuild.ModificationIds, cancellationToken);
        if (!getPriceResult.IsSuccess) return getPriceResult.MapFailure<CartItemDTO>();
        

        // new cart item to add
        CartItem newCartItem = new CartItem()
        {
            Id = Guid.NewGuid(),
            OrderType = OrderTypeEnum.Custom,
            UserId = userId,
            CustomBuildId = addCustomBuildResult.Value!.Id,
            CreatedAt = DateTime.UtcNow,
            OrderPrice = getPriceResult.Value!
        };

        // adding item to the cart
        cartItemRepository.Add(newCartItem);
        
        if (!await cartItemRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} Failed saving changes toDB",
                nameof(CartItemService), nameof(AddCustomBuildAsync));
            return Result<CartItemDTO>.Failure("Failed saving changes to DB");
        }

        return Result<CartItemDTO>.Success(newCartItem.ToDTO());
    }
    
    public async Task<Result<IReadOnlyList<CartItemDTO>>> LazyGetAllCartItemsAsync(
        LazyDTO lazyData,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // get user cart items
        var cartItems = await cartItemRepository.LazyGetCartItemsAsync(lazyData, userId, cancellationToken);

        return Result<IReadOnlyList<CartItemDTO>>.Success(cartItems.Select(ci => ci.ToDTO()).ToList());
    }
    
    public async Task<Result<CartSummaryDTO>> GetSummaryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var summary = await cartItemRepository.GetSummaryInfoAsync(userId, cancellationToken);
        if(summary == null)
            return  Result<CartSummaryDTO>.Failure("No info found");

        return Result<CartSummaryDTO>.Success(summary);
    }
    
    public async Task<Result> RemoveByIdAsync(Guid cartItemId, Guid currUserId, CancellationToken cancellationToken = default)
    {
        // removed item
        var cartItem = await cartItemRepository.RemoveByIdAsync(cartItemId, cancellationToken);;
        if (cartItem == null)
        {
            logger.LogError("{serviceName}.{methodName} cart item with id: {cartItemId} was not found",
                nameof(CartItemService), nameof(RemoveByIdAsync), cartItemId);
            return  Result.Failure("Item not found");
        }

        if (currUserId != cartItem.UserId)
        {
            logger.LogWarning("{serviceName}.{methodName} user with id: {userId} tried removing other user cart item",
                nameof(CartItemService), nameof(RemoveByIdAsync), currUserId);
            return  Result.Failure("Unauthorized", HttpStatusCode.Unauthorized);
        }
        
        if (!await cartItemRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} Failed saving changes toDB",
                nameof(CartItemService), nameof(RemoveByIdAsync));
            return Result.Failure("Failed saving changes to DB");
        }
        
        return Result.Success();
    }
    
    public async Task<Result<IReadOnlyList<CartItemDTO>>> UpdateQuantitiesAsync(
        IReadOnlyList<Id_Quantity_DTO_ts> needsUpdate,
        Guid currUserId,
        CancellationToken cancellationToken = default)
    {
        // set for faster check for ids
        var ids = needsUpdate.Select(nu => nu.ItemId);

        // get items needed to be updated
        var items = await cartItemRepository.FilterAsync(ci => ids.Contains(ci.Id), [], cancellationToken);
        
        if(items.Count != needsUpdate.Count)
            return Result<IReadOnlyList<CartItemDTO>>.Failure("some cart items were not found");

        // if user isnt the owner of any cart item stop
        if(items.Any(i => i.UserId != currUserId))
            return Result<IReadOnlyList<CartItemDTO>>.Failure("Unauthorized", HttpStatusCode.Unauthorized);
        
        // update items
        var updated = await cartItemRepository.UpdateQuantitiesAsync(needsUpdate, cancellationToken);
        
        // save changes
        if (!await cartItemRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} Failed saving changes toDB",
                nameof(CartItemService), nameof(UpdateQuantitiesAsync));
            return Result<IReadOnlyList<CartItemDTO>>.Failure("Failed saving changes to DB");
        }

        return Result<IReadOnlyList<CartItemDTO>>.Success(updated.Select(u => u.ToDTO()).ToList());
    }
}