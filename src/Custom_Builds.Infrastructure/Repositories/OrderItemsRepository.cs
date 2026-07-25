using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;

namespace Custom_Builds.Infrastructure.Repositories;

public class OrderItemsRepository(ApplicationDbContext dbContext) : IOrderItemsRepository
{
    public void AddRange(IEnumerable<OrderItem> orderItems)
    {
        dbContext.AddRangeAsync(orderItems);
    }

    public void Add(OrderItem orderItem)
    {
        dbContext.Add(orderItem);
    }

    public async Task<IReadOnlyList<OrderItem>> FilterAsync(
        Expression<Func<OrderItem, bool>> predicate,
        Expression<Func<OrderItem, object?>>[]? include = null,
        int ? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.OrderItems.AsNoTracking().AsQueryable();

        if (include != null)
        {
            foreach (var inc in include)
            {
                query = query.Include(inc);
            }
        }

        query = query.Where(predicate);
        
        if(skip != null) query = query.Skip(skip.Value);
        if(take != null) query = query.Take(take.Value);
        
        return await query.ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<OrderItem, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return dbContext.OrderItems.AsNoTracking().CountAsync(predicate, cancellationToken);
    }

    public async Task<OrderItem?> GetByIdAsync(Guid orderItemId,Expression<Func<OrderItem, object?>>[]? include, CancellationToken cancellationToken = default)
    {
        var query = dbContext.OrderItems.AsNoTracking().AsQueryable();

        if (include != null)
        {
            foreach (var inc in include)
            {
                query = query.Include(inc);
            }
        }
        
        return await query.SingleOrDefaultAsync(oi => oi.Id == orderItemId, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await  dbContext.SaveChangesAsync(cancellationToken) > 0;  
    }
}