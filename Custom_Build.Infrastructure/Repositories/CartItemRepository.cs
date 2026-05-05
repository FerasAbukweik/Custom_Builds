using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CartItem>> AddAsync(CartItem toAdd)
        {
            if(toAdd.CustomBuildId == null && toAdd.ProductId == null)
            {
                return Result<CartItem>.Failure($"either {nameof(toAdd.ProductId)} or {nameof(toAdd.CustomBuildId)} must be provided");
            }

            if (toAdd.CustomBuildId != null && toAdd.ProductId != null)
            {
                return Result<CartItem>.Failure($"both {nameof(toAdd.ProductId)} and {nameof(toAdd.CustomBuildId)} cant be provided at the same time");
            }

            CartItem newCartItem = new CartItem()
            {
                Id = Guid.NewGuid(),
                UserId = toAdd.UserId,
                orderType = toAdd.orderType,
                CustomBuildId = toAdd.CustomBuildId,
                ProductId = toAdd.ProductId
            };

            _dbContext.Cart.Add(newCartItem);
            await _dbContext.SaveChangesAsync();

            return Result<CartItem>.Success(newCartItem);
        }
        public async Task<Result<CartItem>> GetByIdAsync(Guid cartItemId)
        {
            CartItem? cartItem = await _dbContext.Cart.FirstOrDefaultAsync(c => c.Id == cartItemId);

            if (cartItem == null)
            {
                return Result<CartItem>.Failure("cart item wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<CartItem>.Success(cartItem);
        }
        public async Task<Result> RemoveAsync(CartItem toDel)
        {
            _dbContext.Cart.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> RemoveByIdAsync(Guid cartItemId)
        {
            CartItem? toDel = await _dbContext.Cart.FirstOrDefaultAsync(c => c.Id == cartItemId);

            if (toDel == null)
            {
                return Result.Failure("cart item wasnt found", statusCode: HttpStatusCode.NotFound);
            }


            _dbContext.Cart.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<List<CartItem>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData)
        {
            List<CartItem> cartItems = await _dbContext.Cart.Where(c => c.UserId == getData.UserId)
                .Skip(getData.Section * getData.ElementsPerSection)
                .Take(getData.ElementsPerSection)
                .ToListAsync();

            return Result<List<CartItem>>.Success(cartItems);
        }
        public async Task<Result<List<CartItem>>> FilterAsync(Expression<Func<CartItem, bool>> extraChecks, Expression<Func<CartItem, object>>[]? includes = null)
        {
            var CartItemQuery = _dbContext.Cart.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    CartItemQuery = CartItemQuery.Include(include);
                }
            }

            List<CartItem> cartItems = await CartItemQuery.Where(extraChecks).ToListAsync();

            return Result<List<CartItem>>.Success(cartItems);
        }
    }
}