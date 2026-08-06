using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts;

public interface IOrderItemsRepository
{
    void AddRange(IEnumerable<OrderItem> orderItems);
    
    void Add(OrderItem orderItem);

    Task<IReadOnlyList<OrderItem>> FilterAsync(
        Expression<Func<OrderItem, bool>> predicate,
        Expression<Func<OrderItem, object?>>[]? include = null,
        Expression<Func<OrderItem, object?>>? orderBy = null,
        bool orderByDescending = false,
        int ? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(Expression<Func<OrderItem, bool>> predicate, CancellationToken cancellationToken = default);
    
    Task<OrderItem?> GetByIdAsync(Guid orderItemId,Expression<Func<OrderItem, object?>>[]? include, CancellationToken cancellationToken = default);
    
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}