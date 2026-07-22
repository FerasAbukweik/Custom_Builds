using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Cart;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CartItemRepository(ApplicationDbContext dbContext) : ICartItemRepository
    {
        public void Add(CartItem toAdd)
        {
            dbContext.CartItems.Add(toAdd);
        }
        public async Task<CartItem?> GetByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default)
        {
            return await dbContext.CartItems.FindAsync([cartItemId], cancellationToken);
        }
        public async Task<CartItem?> RemoveByIdAsync(Guid cartItemId, CancellationToken cancellationToken = default)
        {
            CartItem? toDel = await GetByIdAsync(cartItemId, cancellationToken);

            if(toDel != null) dbContext.CartItems.Remove(toDel);

            return toDel;
        }
        public async Task<List<CartItem>> FilterAsync(
            Expression<Func<CartItem, bool>> extraChecks,
            Expression<Func<CartItem, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.CartItems.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }
        public void UpdateRange(List<CartItem> newItems)
        {
            dbContext.CartItems.UpdateRange(newItems);
        }
        public async Task<List<CartItem>> LazyGetCartItemsAsync(LazyDTO reqData, Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .OrderBy(o => o.CreatedAt)
                .Skip(reqData.Taken)
                .Take(reqData.SectionSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<CartSummaryDTO?> GetSummaryInfoAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.CartItems
                .Where(ci => ci.UserId == userId)
                .GroupBy(ci => 1)
                .Select(g => new CartSummaryDTO
                {
                    TotalOrders = g.Sum(ci => ci.Quantity),
                    TotalPrice = g.Sum(ci =>
                        (ci.Product != null ? ci.Product.Price : 0) * ci.Quantity +
                        (ci.CustomBuild != null ? ci.CustomBuild.Modifications.Sum(m => m.Price) : 0) * ci.Quantity
                    )
                }).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<CartItem>> UpdateQuantitiesAsync(IReadOnlyList<Id_Quantity_DTO_ts> needsUpdate, CancellationToken cancellationToken = default)
        {
            var needsUpdateIds = needsUpdate.Select(nu => nu.ItemId);
            var toUpdate = await dbContext.CartItems.Where(ci => needsUpdateIds.Contains(ci.Id)).ToListAsync(cancellationToken);

            var dic = needsUpdate.ToDictionary(
                x => x.ItemId,
                x => x.NewQuantity
            );
            
            foreach (var cartItem in toUpdate)
            {
                cartItem.Quantity = dic[cartItem.Id];
            }

            return toUpdate;
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await dbContext.SaveChangesAsync(cancellationToken)) > 0;
        }
    }
}