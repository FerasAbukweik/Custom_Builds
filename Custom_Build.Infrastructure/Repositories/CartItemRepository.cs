using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<CartItem>> AddAsync(AddCartItemToDB_DTO toAdd)
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
                TotalPrice = toAdd.TotalPrice,
                orderType = toAdd.orderType,
                CustomBuildId = toAdd.CustomBuildId,
                ProductId = toAdd.ProductId
            };

            _dbContext.Cart.Add(newCartItem);
            await _dbContext.SaveChangesAsync();

            return Result<CartItem>.Success(newCartItem);
        }
        public async Task<Result> EditByIdAsync(EditCartItemDTO newData)
        {
            CartItem? toEdit = await _dbContext.Cart.FirstOrDefaultAsync(c => c.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("cart item wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            if(newData.CustomBuildId == null && newData.ProductId == null)
            {
                return Result.Failure($"either {nameof(newData.ProductId)} or {nameof(newData.CustomBuildId)} must be provided");
            }

            if(newData.CustomBuildId != null && newData.ProductId != null)
            {
                return Result.Failure($"both {nameof(newData.ProductId)} and {nameof(newData.CustomBuildId)} cant be provided at the same time");
            }

            toEdit.UserId = newData.UserId ?? toEdit.UserId;
            toEdit.TotalPrice = newData.TotalPrice ?? toEdit.TotalPrice;
            toEdit.orderType = newData.orderType ?? toEdit.orderType;
            toEdit.CustomBuildId = newData.CustomBuildId ?? toEdit.CustomBuildId;
            toEdit.ProductId = newData.ProductId ?? toEdit.ProductId;

            await _dbContext.SaveChangesAsync();
            return Result.Success();
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
        public async Task<Result<List<MiniCartItemDTO>>> GetAllCartItemsAsync(LazyGetCartItemsDTO getData)
        {
            List<MiniCartItemDTO> cartItems = await _dbContext.Cart.Where(c => c.UserId == getData.UserId)
                .Skip(getData.Section * getData.ElementsPerSection)
                .Take(getData.ElementsPerSection)
                .Select(c => new MiniCartItemDTO()
                {
                    Id = c.Id,
                    TotalPrice = c.TotalPrice,
                    orderType = c.orderType,
                    CustomBuildId = c.CustomBuildId,
                    ProductId = c.ProductId
                }).ToListAsync();

            return Result<List<MiniCartItemDTO>>.Success(cartItems);
        }
        
    }
}
