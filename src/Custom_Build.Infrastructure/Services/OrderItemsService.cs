using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class OrderItemsService(
    IOrderItemsRepository orderItemsRepository,
    ICustomBuildService customBuildService,
    IOrderService orderService,
    ILogger<OrderItemsService> logger) : IOrderItemsService
{
    public async Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetCompletedOrderItemsAsync(
        Guid userId,
        LazyDTO lazyData,
        CancellationToken cancellationToken = default
        )
    {
        var result =
            await orderItemsRepository
                .FilterAsync(oi => (oi.Order!.UserId == userId && 
                                        (
                                            oi.Order.OrderStatus == OrderStateEnum.Cancelled ||
                                            oi.Order.OrderStatus == OrderStateEnum.Completed ||
                                            oi.Order.OrderStatus == OrderStateEnum.Refunded ||
                                            oi.Order.OrderStatus == OrderStateEnum.Rejected ||
                                            oi.Order.OrderStatus == OrderStateEnum.Returned ||
                                            oi.Order.OrderStatus == OrderStateEnum.Shipped
                                        )
                                    ),
                    [oi => oi.Product, // used for DTO
                    oi => oi.CustomBuild], // used for DTO
                    lazyData.Taken,
                    lazyData.SectionSize,
                    cancellationToken);

        return Result<IReadOnlyList<OrderItemDTO>>.Success(result.Select(r => r.ToDTO()).ToList());
    }
    
    public async Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetProcessingOrderItemsAsync(
        Guid userId,
        LazyDTO lazyData,
        CancellationToken cancellationToken = default
    )
    {
        var result =
            await orderItemsRepository
                .FilterAsync(oi => (oi.Order!.UserId == userId && 
                                    (
                                        oi.Order.OrderStatus == OrderStateEnum.Processing ||
                                        oi.Order.OrderStatus == OrderStateEnum.Testing
                                    )
                        ),
                    [oi => oi.Product, // used for DTO
                        oi => oi.CustomBuild], // used for DTO
                    lazyData.Taken,
                    lazyData.SectionSize,
                    cancellationToken);

        return Result<IReadOnlyList<OrderItemDTO>>.Success(result.Select(r => r.ToDTO()).ToList());
    }

    public async Task<Result<int>> GetProcessingItemsCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return Result<int>.Success(await orderItemsRepository.CountAsync(oi => (oi.Order!.UserId == userId && 
                (
                    oi.Order.OrderStatus == OrderStateEnum.Processing ||
                    oi.Order.OrderStatus == OrderStateEnum.Testing
                )
            ), cancellationToken));
    }

    public async Task<Result<OrderItemDTO>> BuyAgainAsync(Guid userId, Guid orderItemId, CancellationToken cancellationToken = default)
    {
        var oldOrderItem = await orderItemsRepository.GetByIdAsync(orderItemId,[oi => oi.Product, oi => oi.CustomBuild], cancellationToken);
        if (oldOrderItem == null)
        {
            logger.LogWarning("{serviceName}.{methodName} order item with id: {orderId} was not found",
                nameof(OrderItemsService), nameof(BuyAgainAsync), orderItemId);
            return Result<OrderItemDTO>.Failure("order item was not found");
        }

        var getPriceResult = await GetPrice(oldOrderItem, cancellationToken);
        if (!getPriceResult.IsSuccess) return getPriceResult.MapFailure<OrderItemDTO>();
        
        // add new order
        var addOrderResult = await orderService.AddAsync(userId, cancellationToken);
        if (!addOrderResult.IsSuccess) return addOrderResult.MapFailure<OrderItemDTO>();

        var newOrderItem = new OrderItem()
        {
            OrderType = oldOrderItem.OrderType,
            OrderedPrice = getPriceResult.Value,
            Quantity = 1,
            CustomBuildId = oldOrderItem.CustomBuildId,
            ProductId = oldOrderItem.ProductId,
            OrderId = addOrderResult.Value!.Id
        };
        
        orderItemsRepository.Add(newOrderItem);

        if (!await orderItemsRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(OrderItemsService), nameof(BuyAgainAsync));
            return Result<OrderItemDTO>.Failure("failed saving changes to DB");
        }
        
        // for DTO
        newOrderItem.Product = oldOrderItem.Product;
        newOrderItem.CustomBuild = oldOrderItem.CustomBuild;

        return Result<OrderItemDTO>.Success(newOrderItem.ToDTO());
    }

    private async Task<Result<decimal>> GetPrice(OrderItem orderItem, CancellationToken cancellationToken = default)
    {
        switch (orderItem.OrderType)
        {
            case OrderTypeEnum.Custom:
                return await customBuildService.GetPriceAsync(orderItem.CustomBuildId!.Value, cancellationToken);
            
            case OrderTypeEnum.Product:
                return Result<decimal>.Success(orderItem.Product!.Price);
            
            default:
                logger.LogError("{serviceName}.{methodName} order type: {orderType} not handled",
                    nameof(OrderItemsService), nameof(BuyAgainAsync), orderItem.OrderType.ToString());
                return Result<decimal>.Failure("unhandled order type");
        }
    }
}