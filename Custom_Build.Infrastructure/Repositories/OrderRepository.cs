using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public async Task<Result<Order>> AddAsync(Order toAdd)
        {
            Order newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = toAdd.UserId,
                TotalPrice = toAdd.TotalPrice,
                OrderStatus = OrderStateEnum.Pending,
                OrderType = toAdd.OrderType,
                ProductId = toAdd.ProductId,
                CustomBuildId = toAdd.CustomBuildId,
                Title = toAdd.Title
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return Result<Order>.Success(newOrder);
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
        public async Task<Result<List<MiniOrderInfoDTO>>> GetCompletedUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            List<MiniOrderInfoDTO> orders = await _dbContext.Orders.Where(o => o.UserId == lazyGetUserOrdersData.UserId &&
           (o.OrderStatus == OrderStateEnum.Completed ||
            o.OrderStatus == OrderStateEnum.Returned ||
            o.OrderStatus == OrderStateEnum.Cancelled ||
            o.OrderStatus == OrderStateEnum.Refunded ||
            o.OrderStatus == OrderStateEnum.Rejected))
                .Skip(lazyGetUserOrdersData.Section * lazyGetUserOrdersData.ElementsPerSection)
                .Take(lazyGetUserOrdersData.ElementsPerSection)
                .Select(o => new MiniOrderInfoDTO() 
                {
                    Id = o.Id,
                    Title = o.Title,

                    Image = o.OrderType == OrderTypeEnum.Custom ? 
                    "add image later" : 
                    o.Product == null ? "No Product Image" : o.Product.images.FirstOrDefault() ?? "No Product Image",

                    status = o.OrderStatus,
                    DeliveryDate = o.CreatedAt.AddDays(4), // later add algorithim to determine delivery date based on other orders
                })
                .ToListAsync();

            return Result<List<MiniOrderInfoDTO>>.Success(orders);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetOrdersByUserIdAsync(LazyGetALlOrdersDTO lazyGetOrdersData)
        {
            List<MiniOrderInfoDTO> orders = await _dbContext.Orders.Where(o => o.UserId == lazyGetOrdersData.UserId)
                .Skip(lazyGetOrdersData.Section * lazyGetOrdersData.ElementsPerSection)
                .Take(lazyGetOrdersData.ElementsPerSection)
                .Select(o => new MiniOrderInfoDTO()
                {
                    Id = o.Id,
                    Title = o.Title,

                    Image = o.OrderType == OrderTypeEnum.Custom ?
                    "add image later" :
                    o.Product == null ? "No Product Image" : o.Product.images.FirstOrDefault() ?? "No Product Image",

                    status = o.OrderStatus,
                    DeliveryDate = o.CreatedAt.AddDays(4), // later add algorithim to determine delivery date based on other orders
                })
                .ToListAsync();

            return Result<List<MiniOrderInfoDTO>>.Success(orders);
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
        public async Task<Result<List<Order>>> FilterAsync(Expression<Func<Order, bool>> extraChecks, Expression<Func<Order, object>>[]? includes = null)
        {

            var orderQuery = _dbContext.Orders.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    orderQuery = orderQuery.Include(include);
                }
            }

            List<Order> orders = await orderQuery.Where(extraChecks).ToListAsync();

            return Result<List<Order>>.Success(orders);
        }

    }
}
