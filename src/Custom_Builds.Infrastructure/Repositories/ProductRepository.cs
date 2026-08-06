using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Product;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
    {
        public void Add(Product toAdd)
        {
            dbContext.Products.Add(toAdd);
        }
        public async Task<Product?> EditByIdAsync(ProductEditDTO newData, CancellationToken cancellationToken = default)
        {
            Product? toEdit = await dbContext.Products.FindAsync([newData.Id], cancellationToken);
            
            if (toEdit == null) return null;
            
            toEdit.Title = newData.Name ?? toEdit.Title;
            toEdit.Price = newData.Price ?? toEdit.Price;
            toEdit.Description = newData.Description ?? toEdit.Description;
            toEdit.InStock = newData.InStock ?? toEdit.InStock;

            return toEdit;
        }
        public async Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);
        }
        public async Task<Product?> RemoveByIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            Product? toDel = await GetByIdAsync(productId, cancellationToken);
            
            if (toDel == null) return null;
            
            dbContext.Products.Remove(toDel);

            return toDel;
        }
        public async Task<IReadOnlyList<Product>> FilterAsync(
            Expression<Func<Product, bool>> extraChecks,
            Expression<Func<Product, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Products.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<Product>> LazyGetAllProductsAsync(
            LazyDTO reqData,
            CancellationToken cancellationToken = default)
        {
            return await dbContext.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip(reqData.Taken)
                .Take(reqData.SectionSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IReadOnlyList<MiniInventoryItemDTO>> GetDashboardMiniInfoAsync(
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.Products.AsQueryable();
            
            if(skip != null) query = query.Skip(skip.Value);
            if (take != null) query = query.Take(take.Value);

            return await query.OrderBy(p => p.InStock).Select(p =>
                new MiniInventoryItemDTO()
                {
                    InStock = p.InStock,
                    Title = p.Title,
                }).ToListAsync(cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<Product, bool>> filters, CancellationToken cancellationToken = default)
        {
            return dbContext.Products.CountAsync(filters, cancellationToken);
        }
    }
}
