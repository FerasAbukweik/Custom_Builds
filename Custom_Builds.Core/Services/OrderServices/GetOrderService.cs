using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IOrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<Order>> GetByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }
    }
}
