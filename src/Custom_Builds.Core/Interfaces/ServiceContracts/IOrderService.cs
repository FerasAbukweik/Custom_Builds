
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Order;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IOrderService
{
    Task<Result<OrderDTO>> AddOrderFromCartItemsAsync(Guid currUserId, CancellationToken cancellationToken = default);
    Task<Result<OrderHistoryDTO>> GetOrderHistoryAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<OrderDTO>> AddAsync(Guid userId, CancellationToken cancellationToken = default);
}