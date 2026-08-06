using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICartItemRepository cartItemRepository,
    IOrderItemsRepository orderItemsRepository,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<Result<OrderDTO>> AddOrderWithCartItemsAsync(Guid currUserId, CancellationToken cancellationToken = default)
    {
        var cartItems = await cartItemRepository.FilterAsync(
            ci => ci.UserId == currUserId,
            null,
            null,
            false,
            null,
            null,
            cancellationToken);

        if (cartItems.Count == 0)
        {
            logger.LogWarning("{serviceName}.{methodName} user with id: {currUserId} tried adding order with no items in cart",
                nameof(OrderService), nameof(AddOrderWithCartItemsAsync), cartItems);
            return Result<OrderDTO>.Failure("no items in cart"); 
        }
        
        var newOrder = new Order()
        {
            UserId = currUserId,
            OrderStatus = OrderStateEnum.Processing
        };
        
        orderRepository.Add(newOrder);

        List<OrderItem> orderItems = cartItems.Select(ci => new OrderItem()
        {
            OrderType = ci.OrderType,
            OrderedPrice = ci.OrderPrice,
            Quantity = ci.Quantity,
            OrderId = newOrder.Id,
            CustomBuildId = ci.CustomBuildId,
            ProductId = ci.ProductId,
        }).ToList();
        
        orderItemsRepository.AddRange(orderItems);

        if (!await orderRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(OrderService), nameof(AddOrderWithCartItemsAsync));
            return Result<OrderDTO>.Failure("failed saving changes to DB", HttpStatusCode.InternalServerError);
        }
        
        // clear cart
        await cartItemRepository.ClearCartAsync(cancellationToken);

        // for the DTO
        newOrder.OrderedItems = orderItems;

        return Result<OrderDTO>.Success(newOrder.toDTO());
    }

    public async Task<Result<OrderHistoryDTO>> GetOrderHistoryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await orderRepository.GetHistorySummaryAsync(userId, cancellationToken);
        if (result == null)
            return Result<OrderHistoryDTO>.Success(new OrderHistoryDTO()
            {
                TotalPrice = 0,
                Count = 0
            });
        
        return Result<OrderHistoryDTO>.Success(result); 
    }

    public async Task<Result<decimal>> GetTotalRevenueAsync(CancellationToken cancellationToken = default)
    {
        var result = await orderRepository.GetTotalRevenueAsync(cancellationToken);
        
        return Result<decimal>.Success(result);
    }

    public async Task<Result<int>> GetPendingOrdersCount(CancellationToken cancellationToken = default)
    {
        var pendingStatus = new OrderStateEnum[]
        {
            OrderStateEnum.Processing,
            OrderStateEnum.Testing,
            OrderStateEnum.Shipped
        };
        var result = await orderRepository.CountAsync(o => pendingStatus.Contains(o.OrderStatus), cancellationToken);
        
        return Result<int>.Success(result);
    }

    public async Task<Result<IReadOnlyList<decimal>>> GetDailyRevenueAsync(int days, CancellationToken cancellationToken = default)
    {
        var result =(List<decimal>) await orderRepository.GetDailyRevenueAsync(days, cancellationToken);

        int missingDays = days - result.Count;
        if (missingDays > 0)
            result.AddRange(Enumerable.Repeat(0m, missingDays));
        
        return Result<IReadOnlyList<decimal>>.Success(result);
    }

    public async Task<Result<int>> GetLatestOrdersCountAsync(int days, CancellationToken cancellationToken = default)
    {
        return Result<int>.Success(await orderRepository.CountAsync(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-1 * days), cancellationToken));
    }

    public async Task<Result<IReadOnlyList<OrderDTO>>> LazyGetOrdersAsync(Guid? userId, LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var result = await orderRepository.FilterAsync(
            o => (userId == null || o.UserId == userId.Value),
            [o => o.OrderedItems],
            o => o.CreatedAt,
            true,
            lazyData,
            cancellationToken
        );

        return Result<IReadOnlyList<OrderDTO>>.Success(result.Select(r => r.toDTO()).ToList());
    }
    
    public async Task<Result<IReadOnlyList<OrderDTO>>> LazyGetPendingOrdersAsync(Guid? userId, LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var pendingStatus = new OrderStateEnum[]
        {
            OrderStateEnum.Processing,
            OrderStateEnum.Testing,
            OrderStateEnum.Shipped
        };
        
        var result = await orderRepository.FilterAsync(
            o => ((userId == null || o.UserId == userId.Value) &&  pendingStatus.Contains(o.OrderStatus)),
            [o => o.OrderedItems],
            o => o.CreatedAt,
            true,
            lazyData,
            cancellationToken
        );

        return Result<IReadOnlyList<OrderDTO>>.Success(result.Select(r => r.toDTO()).ToList());
    }

    public async Task<Result<OrderDetailsDto>> GetDetailsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, [o => o.User], cancellationToken);
        if (order == null)
            return Result<OrderDetailsDto>.Failure("Order Not found");

        return Result<OrderDetailsDto>.Success(order.ToDetailsDto());
    }
}