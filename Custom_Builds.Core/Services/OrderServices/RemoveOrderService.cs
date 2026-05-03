using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IOrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class RemoveOrderService : IRemoveOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public RemoveOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.RemoveByIdAsync(orderId);
            if (!result.IsSuccess) return result.MapFailure();

            return Result.Success();
        }
    }
}
