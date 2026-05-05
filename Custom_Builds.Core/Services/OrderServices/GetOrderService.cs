using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IOrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGetCurrUserService _getCurrUserService;

        public GetOrderService(IOrderRepository orderRepository,
                               IGetCurrUserService getCurrUserService)
        {
            _orderRepository = orderRepository;
            _getCurrUserService = getCurrUserService;
        }

        public async Task<Result<Order>> GetByIdAsync(Guid orderId)
        {
            var result = await _orderRepository.GetByIdAsync(orderId);
            if (!result.IsSuccess) return result.MapFailure<Order>();

            return Result<Order>.Success(result.Value!);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetCompletedUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            var getCurrUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrUserIdRes.IsSuccess) getCurrUserIdRes.MapFailure<List<MiniOrderInfoDTO>>();

            // set curr user id into the request data
            lazyGetUserOrdersData.UserId = getCurrUserIdRes.Value!;

            // get the data from DB
            var result = await _orderRepository.GetCompletedUserOrdersAsync(lazyGetUserOrdersData);
            if (!result.IsSuccess) return result.MapFailure<List<MiniOrderInfoDTO>>();


            return Result<List<MiniOrderInfoDTO>>.Success(result.Value!);
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> GetUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
        {
            var getCurrUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrUserIdRes.IsSuccess) getCurrUserIdRes.MapFailure<List<MiniOrderInfoDTO>>();
            
            // set curr user id into the request data
            lazyGetUserOrdersData.UserId = getCurrUserIdRes.Value!;


            // get user orders
            var userOrders = await _orderRepository.GetOrdersByUserIdAsync(lazyGetUserOrdersData);
            if(!userOrders.IsSuccess) return userOrders.MapFailure<List<MiniOrderInfoDTO>>();

            return Result<List<MiniOrderInfoDTO>>.Success(userOrders.Value!);
        }
    }
}
