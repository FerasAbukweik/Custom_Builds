using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
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
        public async Task<Result<List<HistoryOrderDTO>>> GetCompletedOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData)
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
        public async Task<Result<int>> GetOrdersCountAsync(Guid? userId)
        {
            var getTargetUserIdRes = _getCurrUserService.GetTargetUserId(userId);
            if (!getTargetUserIdRes.IsSuccess) getTargetUserIdRes.MapFailure<int>();

            userId = getTargetUserIdRes.Value!;

            var getSumRes = await _orderRepository.GetOrdersCountAsync(userId!.Value);


            return getSumRes;
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

            if (!userOrders.Value!.Any())
                return Result<List<MiniOrderInfoDTO>>.Failure("no orders where found", HttpStatusCode.NotFound);

            return Result<List<MiniOrderInfoDTO>>.Success(userOrders.Value!);
        }
        public async Task<Result<int>> GetAllCompletedOrdersCountAsync(Guid? userId)
        {
            var getTargetUserIdRes = _getCurrUserService.GetTargetUserId(userId);
            if (!getTargetUserIdRes.IsSuccess) getTargetUserIdRes.MapFailure<int>();

            userId = getTargetUserIdRes.Value!;

            var getSumRes = await _orderRepository.GetCompletedOrdersCountAsync(userId!.Value);


            return getSumRes;
        }
    }
}
