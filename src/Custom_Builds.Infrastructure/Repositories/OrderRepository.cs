using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Enums;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
            return await dbContext.Orders.AsNoTracking().CountAsync(checks ,cancellationToken);
        }
        public async Task<OrderHistoryDTO?> GetHistorySummaryAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.AsNoTracking().Where(o => o.UserId == userId)
            .GroupBy(g => 1)
            .Select(g => new OrderHistoryDTO()
            {
                Count = g.Sum(o => o.OrderedItems.Count),
                TotalPrice = g.Sum(o => o.OrderedItems.Sum(oi => (oi.OrderedPrice * oi.Quantity))),
            }).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<Order?> UpdateOrderStatus(Guid orderId, OrderStateEnum newStatus, CancellationToken cancellationToken = default)
        {
            var toEdit = await dbContext.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (toEdit == null) return null;

            toEdit.OrderStatus = newStatus;

            return toEdit;
        }
        public async Task<IReadOnlyList<Order>> FilterAsync(
            Expression<Func<Order, bool>> extraChecks,
            Expression<Func<Order, object?>>[]? includes,
            Expression<Func<Order, object?>>? orderBy,
            bool orderByDescending = false,
            LazyDTO? lazyData = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Orders.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = query.Where(extraChecks);

            if (orderBy != null)
            {
                if(orderByDescending) query = query.OrderByDescending(orderBy);
                else  query = query.OrderBy(orderBy);
            }

            if (lazyData != null)
            {
                query = query.Skip(lazyData.Taken);
                query = query.Take(lazyData.SectionSize);
            }
            
            return await query.ToListAsync(cancellationToken);
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        public async Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.SumAsync(o => o.OrderedItems.Sum(oi => (oi.OrderedPrice * oi.Quantity )), cancellationToken);
        }
        public async Task<IReadOnlyList<decimal>> GetDailyRevenueAsync(int days, CancellationToken cancellationToken = default)
        {
            return await dbContext.Orders.GroupBy(o => o.CreatedAt.Date)
                .Select(g => g.Sum(o => o.OrderedItems.Sum(oi => (oi.OrderedPrice * oi.Quantity ))))
                .ToListAsync(cancellationToken);
;        }

        public async Task<Order?> GetByIdAsync(Guid orderId, Expression<Func<Order, object?>>[]? include = null, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Orders.AsNoTracking().AsQueryable();

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return await query.SingleOrDefaultAsync(o => o.Id == orderId ,cancellationToken);
;        }
    }
}
