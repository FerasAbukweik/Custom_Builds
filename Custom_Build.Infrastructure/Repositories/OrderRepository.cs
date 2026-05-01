using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid>> AddAsync(AddOrderTO_DB toAdd)
        {
            Order newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = toAdd.UserId,
                TotalPrice = toAdd.TotalPrice,
                OrderStatus = OrderStateEnum.Pending,
                OrderType = toAdd.OrderType,
                ProductId = toAdd.ProductId,
                CustomBuildId = toAdd.CustomBuildId
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return Result<Guid>.Success(newOrder.Id);
        }

        public async Task<Result> EditByIdAsync(EditOrderDTO newData)
        {
            Order? toEdit = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("order wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            toEdit.UserId = newData.UserId ?? toEdit.UserId;
            toEdit.TotalPrice = newData.TotalPrice ?? toEdit.TotalPrice;

            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<Order>> GetByIdAsync(Guid orderId)
        {
            Order? order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return Result<Order>.Failure("order wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<Order>.Success(order);
        }

        public async Task<Result> RemoveByIdAsync(Guid orderId)
        {
            Order? toDel = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (toDel == null)
            {
                return Result.Failure("order wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Orders.Remove(toDel);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
