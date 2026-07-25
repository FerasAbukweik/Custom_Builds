using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
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
    public async Task<Result<OrderDTO>> AddOrderFromCartItemsAsync(Guid currUserId, CancellationToken cancellationToken = default)
    {
        var cartItems = await cartItemRepository.FilterAsync(ci => ci.UserId == currUserId,null,null,null,cancellationToken);

        if (cartItems.Count == 0)
        {
            logger.LogWarning("{serviceName}.{methodName} user with id: {currUserId} tried adding order with no items in cart",
                nameof(OrderService), nameof(AddOrderFromCartItemsAsync), cartItems);
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
                nameof(OrderService), nameof(AddOrderFromCartItemsAsync));
            return Result<OrderDTO>.Failure("failed saving changes to DB", HttpStatusCode.InternalServerError);
        }

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

    public async Task<Result<OrderDTO>> AddAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var newOrder = new Order()
        {
            UserId = userId,
            OrderStatus = OrderStateEnum.Processing,
        };
        
        orderRepository.Add(newOrder);

        if (!await orderRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(OrderService), nameof(AddAsync));
            return Result<OrderDTO>.Failure("Failed saving changes to DB");
        }
        
        return Result<OrderDTO>.Success(newOrder.toDTO());
    }
}