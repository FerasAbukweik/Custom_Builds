using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Order;
using Custom_Builds.Core.DTO.Product;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICartItemRepository cartItemRepository,
    IOrderItemsRepository orderItemsRepository,
    ILogger<OrderService> logger,
    IProductRepository productRepository) : IOrderService
{
    public async Task<Result<OrderDTO>> AddOrderWithCartItemsAsync(Guid currUserId, CancellationToken cancellationToken = default)
    {
        // items in the cart
        var cartItems = await cartItemRepository.FilterAsync(
            ci => ci.UserId == currUserId,
            [ci => ci.Product],
            null,
            false,
            null,
            null,
            cancellationToken);

        // if no items in the cart stop
        if (cartItems.Count == 0)
        {
            logger.LogWarning("{serviceName}.{methodName} user with id: {currUserId} tried adding order with no items in cart",
                nameof(OrderService), nameof(AddOrderWithCartItemsAsync), cartItems);
            return Result<OrderDTO>.Failure("no items in cart"); 
        }
        
        // check inStock if the item is product
        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product != null && cartItem.Product.InStock < cartItem.Quantity)
            {
                return Result<OrderDTO>.Failure($"{cartItem.Product.Title} is low in stock");
            }
        }
        
        // update inStock amount for products
        foreach (var cartItem in cartItems)
        {
            if(cartItem.Product == null) continue;

            productRepository.EditByIdAsync(new ProductEditDTO()
            {
                Id = cartItem.Product.Id,
                InStock = cartItem.Product.InStock - cartItem.Quantity
            });
        }
        
        // new order entity
        var newOrder = new Order()
        {
            UserId = currUserId,
            OrderStatus = OrderStateEnum.Processing
        };
        
        // link the same cart items with the new order
        List<OrderItem> orderItems = cartItems.Select(ci => new OrderItem()
        {
            OrderId = newOrder.Id,
            OrderType = ci.OrderType,
            OrderedPrice = ci.OrderPrice,
            Quantity = ci.Quantity,
            CustomBuildId = ci.CustomBuildId,
            ProductId = ci.ProductId,
        }).ToList();
        
        // add items to the local DB
        orderRepository.Add(newOrder);
        orderItemsRepository.AddRange(orderItems);

        // save changes to DB
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
    public async Task<Result<int>> GetPendingOrdersCount(Guid? currUserId, CancellationToken cancellationToken = default)
    {
        var pendingStatus = new OrderStateEnum[]
        {
            OrderStateEnum.Processing,
            OrderStateEnum.Testing,
            OrderStateEnum.Shipped
        };
        var result = await orderRepository.CountAsync(o => (
            pendingStatus.Contains(o.OrderStatus) && (currUserId == null || o.UserId == currUserId)
            ),
            cancellationToken);
        
        return Result<int>.Success(result);
    }
    public async Task<Result<IReadOnlyList<decimal>>> GetDailyRevenueAsync(int days, CancellationToken cancellationToken = default)
    {
        var result =(List<decimal>) await orderRepository.GetDailyRevenueAsync(days, cancellationToken);

        int missingDays = days - result.Count;
        if (missingDays > 0)
        {
            var tempRes = result;
            result = [];
            result.AddRange(Enumerable.Repeat(0m, missingDays));
            result.AddRange(tempRes);
        }
        
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

    public async Task<Result<OrderDetailsDto>> GetDetailsAsync(Guid orderId,Guid? currUserId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, [o => o.User], cancellationToken);
        if (order == null)
            return Result<OrderDetailsDto>.Failure("Order Not found");
        
        if(currUserId != null && order.UserId != currUserId)
            return Result<OrderDetailsDto>.Failure("Unauthorized", HttpStatusCode.Unauthorized);

        return Result<OrderDetailsDto>.Success(order.ToDetailsDto());
    }

    public async Task<Result<OrderDTO>> UpdateStatus(Guid orderId, OrderStateEnum newStatus, CancellationToken cancellationToken = default)
    {
        var updated = await orderRepository.UpdateOrderStatus(orderId, newStatus, cancellationToken);

        if (updated == null) return Result<OrderDTO>.Failure("Failed Updating order status");
        
        // save changes to DB
        if (!await orderRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(OrderService), nameof(UpdateStatus));
            return Result<OrderDTO>.Failure("failed saving changes to DB", HttpStatusCode.InternalServerError);
        }
        
        return Result<OrderDTO>.Success(updated.toDTO());
    }
}