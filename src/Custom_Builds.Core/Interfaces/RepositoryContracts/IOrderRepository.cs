using System.Linq.Expressions;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IOrderRepository
    {
        void Add(Order toAdd);
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<Order, bool>> checks, CancellationToken cancellationToken = default);
        Task<OrderHistoryDTO?> GetHistorySummaryAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Order?> UpdateOrderStatus(Guid orderId, OrderStateEnum newStatus,
            CancellationToken cancellationToken = default);
        Task<Order?> RemoveByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<OrderDTO>> GetOrdersAsync(
            Guid userId,
            LazyDTO lazyGetUserOrdersData,
            bool isCompleted,
            CancellationToken cancellationToken = default);
        Task<Result<List<Order>>> FilterAsync(
            Expression<Func<Order, bool>> extraChecks,
            Expression<Func<Order, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
