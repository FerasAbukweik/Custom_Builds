
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IOrderService
{
    Task<Result<OrderDTO>> AddOrderWithCartItemsAsync(Guid currUserId, CancellationToken cancellationToken = default);
    Task<Result<OrderHistoryDTO>> GetOrderHistoryAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<decimal>> GetTotalRevenueAsync(CancellationToken cancellationToken = default);
    Task<Result<int>> GetPendingOrdersCount(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<decimal>>> GetDailyRevenueAsync(int days, CancellationToken cancellationToken = default);
    Task<Result<int>> GetLatestOrdersCountAsync(int days, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<OrderDTO>>> LazyGetOrdersAsync(Guid? userId ,LazyDTO lazyData, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<OrderDTO>>> LazyGetPendingOrdersAsync(Guid? userId, LazyDTO lazyData, CancellationToken cancellationToken = default);
    Task<Result<OrderDetailsDto>> GetDetailsAsync (Guid orderId, CancellationToken cancellationToken = default);
}