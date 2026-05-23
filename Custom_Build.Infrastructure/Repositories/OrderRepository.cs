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
        public async Task<Result<int>> GetProcessingOrdersCountAsync(Guid userId)
        {
            var sum = await _dbContext.Orders.Where(o => (
            o.UserId == userId && 
          !(o.OrderStatus == OrderStateEnum.Completed ||
            o.OrderStatus == OrderStateEnum.Returned ||
            o.OrderStatus == OrderStateEnum.Cancelled ||
            o.OrderStatus == OrderStateEnum.Refunded ||
            o.OrderStatus == OrderStateEnum.Rejected))).CountAsync();


            return Result<int>.Success(sum);
        }
        public async Task<Result<OrderHistoryDTO>> GetHistorySummaryAsync(Guid userId)
        {
            OrderHistoryDTO? res = await _dbContext.Orders.Where(o => (
            o.UserId == userId &&
           (o.OrderStatus == OrderStateEnum.Completed ||
            o.OrderStatus == OrderStateEnum.Returned ||
            o.OrderStatus == OrderStateEnum.Cancelled ||
            o.OrderStatus == OrderStateEnum.Refunded ||
            o.OrderStatus == OrderStateEnum.Rejected)))
            .GroupBy(g => 1)
            .Select(g => new OrderHistoryDTO()
            {
                Count = g.Count(),
                TotalPrice = g.Sum(o => ((o.Product != null ? o.Product.Price : 0) +
                    (o.CustomBuild != null ? o.CustomBuild.Modifications.Sum(m => m.Price) : 0)) * o.Quantity)
            }).FirstOrDefaultAsync();

            if(res == null)
            {
                return Result<OrderHistoryDTO>.Failure("didnt find user orders" , HttpStatusCode.NotFound);
            }

            return Result<OrderHistoryDTO>.Success(res);
        }
        public async Task<Result> EditByIdAsync(EditOrderDTO newData)
        {
            Order? toEdit = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("order wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            toEdit.UserId = newData.UserId ?? toEdit.UserId;
            toEdit.Title = newData.Title ?? toEdit.Title;
            toEdit.Quantity = newData.Quantity ?? toEdit.Quantity;
            toEdit.OrderStatus = newData.OrderStatus ?? toEdit.OrderStatus;


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
        public async Task<Result<List<HistoryOrderDTO>>> GetCompletedOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            List<HistoryOrderDTO> orders = await _dbContext.Orders.Where(o => (
                o.UserId == lazyGetUserOrdersData.UserId &&
               (o.OrderStatus == OrderStateEnum.Completed ||
                o.OrderStatus == OrderStateEnum.Returned ||
                o.OrderStatus == OrderStateEnum.Cancelled ||
                o.OrderStatus == OrderStateEnum.Refunded ||
                o.OrderStatus == OrderStateEnum.Rejected)))
                .OrderBy(o => o.CreatedAt)
                .Skip(lazyGetUserOrdersData.Taken)
                .Take(lazyGetUserOrdersData.ElementsPerSection)
                .Select(o => new HistoryOrderDTO() 
                {
                    Id = o.Id,
                    Title = o.Title,
                    Image = o.CustomBuild != null ? "add image later" : 
                    o.Product != null ? o.Product.images.FirstOrDefault() ?? "No Product Image" : "No Product",
                    status = o.OrderStatus,
                    DeliveryDate = o.CreatedAt.AddDays(4), // later add algorithim to determine delivery date based on other orders
                    Quantity = o.Quantity,
                    TotalPrice = ((o.Product != null ? o.Product.Price : 0) +
                    (o.CustomBuild != null ? o.CustomBuild.Modifications.Sum(m => m.Price) : 0)
                    ) * o.Quantity,
                    specs = (o.CustomBuild != null ? o.CustomBuild.Modifications.Select(cb => cb.Name).ToList() :
                    (o.Product != null ? new List<string> { "Product" } : new List<string> { "something went wrong" })),
                })
                .ToListAsync();

            return Result<List<HistoryOrderDTO>>.Success(orders);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetProcessingOrdersAsync(LazyGetALlOrdersDTO lazyGetOrdersData)
        {
            List<MiniOrderInfoDTO> orders = await _dbContext.Orders.Where(o => (
            o.UserId == lazyGetOrdersData.UserId && 
          !(o.OrderStatus == OrderStateEnum.Completed ||
            o.OrderStatus == OrderStateEnum.Returned ||
            o.OrderStatus == OrderStateEnum.Cancelled ||
            o.OrderStatus == OrderStateEnum.Refunded ||
            o.OrderStatus == OrderStateEnum.Rejected)))
                .OrderBy(o => o.CreatedAt)
                .Skip(lazyGetOrdersData.Taken)
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
        public async Task<Result> BuyAgainAsync(Guid orderId , Guid userId)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if(order == null)
            {
                return Result.Failure("Order Wasnt Found");
            }

            Order newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                OrderStatus = OrderStateEnum.Processing,
                CreatedAt = DateTime.UtcNow,
                CustomBuildId = order.CustomBuildId,
                UserId = userId,
                ProductId = order.ProductId,
                OrderType = order.OrderType,
                Quantity = 1,
                Title = order.Title,
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
