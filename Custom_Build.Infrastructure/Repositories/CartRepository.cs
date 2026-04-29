using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> AddAsync(AddCartDTO toAdd)
        {
            Cart newCart = new Cart()
            {
                Id = Guid.NewGuid(),
                UserId = toAdd.UserId,
                TotalPrice = toAdd.TotalPrice
            };

            _dbContext.Carts.Add(newCart);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> EditByIdAsync(EditCartDTO newData)
        {
            Cart? toEdit = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("cart wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            toEdit.UserId = newData.UserId ?? toEdit.UserId;
            toEdit.TotalPrice = newData.TotalPrice ?? toEdit.TotalPrice;

            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<Cart>> GetByIdAsync(Guid cartId)
        {
            Cart? cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
            {
                return Result<Cart>.Failure("cart wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<Cart>.Success(cart);
        }

        public async Task<Result> RemoveByIdAsync(Guid cartId)
        {
            Cart? toDel = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);

            if (toDel == null)
            {
                return Result.Failure("cart wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Carts.Remove(toDel);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
