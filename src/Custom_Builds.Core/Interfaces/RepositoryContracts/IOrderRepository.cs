using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IOrderRepository
    {
        void Add(Order toAdd);
        Task<int> CountAsync(Expression<Func<Order, bool>> checks, CancellationToken cancellationToken = default);
        Task<OrderHistoryDTO?> GetHistorySummaryAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Order?> UpdateOrderStatus(Guid orderId, OrderStateEnum newStatus,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> FilterAsync(
            Expression<Func<Order, bool>> extraChecks,
            Expression<Func<Order, object?>>[]? includes,
            Expression<Func<Order, object?>>? orderBy,
            bool orderByDescending = false,
            LazyDTO? lazyData = null,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<decimal>> GetDailyRevenueAsync(int days, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(Guid orderId,Expression<Func<Order, object?>>[]? include = null, CancellationToken cancellationToken = default);
     }
}
