using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
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
            var result = await _orderRepository.GetByIdAsync(orderId);
            if (!result.IsSuccess) return result.MapFailure<Order>();

            return Result<Order>.Success(result.Value!);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetCompletedUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            var result = await _orderRepository.GetCompletedUserOrdersAsync(lazyGetUserOrdersData);
            if (!result.IsSuccess) return result.MapFailure<List<MiniOrderInfoDTO>>();


            return Result<List<MiniOrderInfoDTO>>.Success(result.Value!);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            // get user orders
            var userOrders = await _orderRepository.GetOrdersByUserIdAsync(lazyGetUserOrdersData);
            if(!userOrders.IsSuccess) return userOrders.MapFailure<List<MiniOrderInfoDTO>>();

            return Result<List<MiniOrderInfoDTO>>.Success(userOrders.Value!);
        }
    }
}
