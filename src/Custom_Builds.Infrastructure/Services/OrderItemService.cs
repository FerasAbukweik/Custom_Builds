using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.OrderItem;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;

namespace Custom_Builds.Infrastructure.Services;

public class OrderItemService(
    IOrderItemsRepository orderItemsRepository
    ) : IOrderItemService
{
    public async Task<Result<IReadOnlyList<OrderItemDTO>>> LazyGetOrderItemsAsync(Guid orderId, Guid? currUserId,LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var result = await orderItemsRepository.FilterAsync(
            (o => (currUserId == null || o.Order!.UserId == currUserId) &&  o.OrderId == orderId),
            [o => o.Product, o => o.CustomBuild, o => o.Order],
            o => o.Order!.CreatedAt,
            true,
            lazyData.Taken,
            lazyData.SectionSize,
            cancellationToken
        );

        return Result<IReadOnlyList<OrderItemDTO>>.Success(result.Select(r => r.ToDTO()).ToList());
    }
}