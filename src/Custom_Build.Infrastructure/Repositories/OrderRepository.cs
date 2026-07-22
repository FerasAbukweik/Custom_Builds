using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Enums;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
    {
        public void Add(Order toAdd)
        {
            dbContext.Orders.Add(toAdd);
        }
        public async Task<int> CountAsync(Expression<Func<Order, bool>> checks , CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.CountAsync(checks ,cancellationToken);
        }
        public async Task<OrderHistoryDTO?> GetHistorySummaryAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.Where(o => (
            o.UserId == userId &&
           (o.OrderStatus == OrderStateEnum.Completed ||
            o.OrderStatus == OrderStateEnum.Returned ||
            o.OrderStatus == OrderStateEnum.Cancelled ||
            o.OrderStatus == OrderStateEnum.Refunded ||
            o.OrderStatus == OrderStateEnum.Rejected)))
            .GroupBy(g => 1)
            .Select(g => new OrderHistoryDTO()
            {
                Count = g.Sum(o => o.OrderedItems.Count),
                TotalPrice = g.Sum(o => o.OrderedItems.Sum(oi => oi.OrderedPrice)),
            }).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<Order?> UpdateOrderStatus(Guid orderId, OrderStateEnum newStatus, CancellationToken cancellationToken = default)
        {
            var toEdit = await dbContext.Orders.FindAsync([orderId], cancellationToken);

            if (toEdit == null) return null;

            toEdit.OrderStatus = newStatus;

            return toEdit;
        }
        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.FindAsync([orderId], cancellationToken);
        }
        public async Task<IReadOnlyList<OrderDTO>> GetOrdersAsync(
            Guid userId,
            LazyDTO lazyGetUserOrdersData,
            bool isCompleted,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Orders.Where(o => o.UserId == userId);

            var completedStatuses = new[] 
            { 
                OrderStateEnum.Completed, 
                OrderStateEnum.Returned, 
                OrderStateEnum.Cancelled, 
                OrderStateEnum.Refunded, 
                OrderStateEnum.Rejected 
            };
            
            if (isCompleted)
                query = query.Where(o => completedStatuses.Contains(o.OrderStatus));
            else
                query = query.Where(o => !completedStatuses.Contains(o.OrderStatus));
            
            return await query.Where(o => o.UserId == userId)
                .OrderBy(o => o.CreatedAt)
                .Skip(lazyGetUserOrdersData.Taken)
                .Take(lazyGetUserOrdersData.SectionSize)
                .Select(o => new OrderDTO() 
                {
                    Id = o.Id,
                    OrderedPrice = o.OrderedItems.Sum(oi => oi.OrderedPrice),
                    OrderStatus = o.OrderStatus,
                    OrderedItems = o.OrderedItems.Select(oi => oi.ToDTO()).ToList(),
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<Order?> RemoveByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            Order? toDel = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (toDel == null) return null;

            dbContext.Orders.Remove(toDel);

            return toDel;
        }
        public async Task<Result<List<Order>>> FilterAsync(
            Expression<Func<Order, bool>> extraChecks,
            Expression<Func<Order, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Orders.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            List<Order> orders = await query.Where(extraChecks).ToListAsync(cancellationToken);

            return Result<List<Order>>.Success(orders);
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
