using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.OrderItem;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IOrderItemsService
{
    Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetCompletedOrderItemsAsync(
        Guid userId,
        LazyDTO lazyData,
        CancellationToken cancellationToken = default
    );

    Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetProcessingOrderItemsAsync(
        Guid userId,
        LazyDTO lazyData,
        CancellationToken cancellationToken = default
    );
    
    Task<Result<int>> GetProcessingItemsCountAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    
    Task<Result<OrderItemDTO>> BuyAgainAsync(
        Guid userId,
        Guid orderItemId,
        CancellationToken cancellationToken = default
    );
}