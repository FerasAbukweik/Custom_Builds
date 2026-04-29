using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RemoveOrderService : IRemoveOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public RemoveOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<Result> RemoveByIdAsync(Guid orderId)
        {
            return _orderRepository.RemoveByIdAsync(orderId);
        }
    }
}
