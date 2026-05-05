using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Product>> AddAsync(Product toAdd)
        {
            _dbContext.Products.Add(toAdd);
            await _dbContext.SaveChangesAsync();

            return Result<Product>.Success(toAdd);
        }
        public async Task<Result> EditByIdAsync(EditProductDTO newData)
        {
            Product? toEdit = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == newData.Id);
            if (toEdit == null) return Result.Failure("product wasnt found", statusCode: HttpStatusCode.NotFound);
            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Price = newData.Price ?? toEdit.Price;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<Product>> GetByIdAsync(Guid productId)
        {
            Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            return product == null
                ? Result<Product>.Failure("product wasnt found", statusCode: HttpStatusCode.NotFound)
                : Result<Product>.Success(product);
        }
        public async Task<Result> RemoveByIdAsync(Guid productId)
        {
            Product? toDel = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (toDel == null) return Result.Failure("product wasnt found", statusCode: HttpStatusCode.NotFound);
            _dbContext.Products.Remove(toDel);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<List<Product>>> FilterAsync(Expression<Func<Product, bool>> extraChecks, Expression<Func<Product, object>>[]? includes = null)
        {

            var productQuery = _dbContext.Products.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    productQuery = productQuery.Include(include);
                }
            }

            List<Product> products = await productQuery.Where(extraChecks).ToListAsync();

            return Result<List<Product>>.Success(products);
        }

    }
}
