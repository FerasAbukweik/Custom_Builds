using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.OrderItem;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IOrderItemService
{
    Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetOrderItemsAsync(
        Guid orderId,
        Guid? currUserId,
        LazyDTO lazyData,
        CancellationToken cancellationToken = default);
}