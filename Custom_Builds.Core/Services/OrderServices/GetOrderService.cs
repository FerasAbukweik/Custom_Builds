using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IOrderServices;
using System.Net;

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
        public async Task<Result<List<HistoryOrderDTO>>> LazyGetCompletedOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData)
        {
            var getCurrUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrUserIdRes.IsSuccess) getCurrUserIdRes.MapFailure<List<MiniOrderInfoDTO>>();

            // set curr user id into the request data
            lazyGetUserOrdersData.UserId = getCurrUserIdRes.Value!;

            // get the data from DB
            var result = await _orderRepository.GetCompletedOrdersAsync(lazyGetUserOrdersData);
            if (!result.IsSuccess) return result.MapFailure<List<HistoryOrderDTO>>();


            return Result<List<HistoryOrderDTO>>.Success(result.Value!);
        }
        public async Task<Result<int>> GetProcessingOrdersCountAsync()
        {
            var getCurrentUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrentUserIdRes.IsSuccess) getCurrentUserIdRes.MapFailure<int>();


            var getSumRes = await _orderRepository.GetProcessingOrdersCountAsync(getCurrentUserIdRes.Value!);


            return getSumRes;
        }
        public async Task<Result<List<MiniOrderInfoDTO>>> LazyGetProcessingOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData)
        {
            var getCurrUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrUserIdRes.IsSuccess) getCurrUserIdRes.MapFailure<List<MiniOrderInfoDTO>>();
            
            // set curr user id into the request data
            lazyGetUserOrdersData.UserId = getCurrUserIdRes.Value!;


            // get user orders
            var userOrders = await _orderRepository.GetProcessingOrdersAsync(lazyGetUserOrdersData);
            if(!userOrders.IsSuccess) return userOrders.MapFailure<List<MiniOrderInfoDTO>>();

            return Result<List<MiniOrderInfoDTO>>.Success(userOrders.Value!);
        }
        public async Task<Result<OrderHistoryDTO>> GetHistorySummaryAsync()
        {
            var getTargetUserIdRes = _getCurrUserService.GetUserId();
            if (!getTargetUserIdRes.IsSuccess) getTargetUserIdRes.MapFailure<int>();

            var getDataRes = await _orderRepository.GetHistorySummaryAsync(getTargetUserIdRes.Value!);


            return getDataRes;
        }
    }
}
