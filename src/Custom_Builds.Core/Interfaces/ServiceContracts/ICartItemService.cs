using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.DTO.CustomBuild;
using Custom_Builds.Core.DTO.Lazy;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface ICartItemService
{
    Task<Result<CartItemDTO>> AddProductAsync(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<CartItemDTO>> AddCustomBuildAsync(
        CustomBuildAddDTO toAddCustomBuild,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<CartItemDTO>>> LazyGetAllCartItemsAsync(
        LazyDTO lazyData,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<CartSummaryDTO>> GetSummaryAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Result> RemoveByIdAsync(Guid cartItemId, Guid currUserId, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<CartItemDTO>>> UpdateQuantitiesAsync(
        IReadOnlyList<Id_Quantity_DTO_ts> needsUpdate,
        Guid currUserId,
        CancellationToken cancellationToken = default);
}