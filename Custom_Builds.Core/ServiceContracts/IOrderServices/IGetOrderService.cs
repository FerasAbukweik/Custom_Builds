using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IOrderServices
{
    public interface IGetOrderService
    {
        Task<Result<Order>> GetByIdAsync(Guid orderId);
        Task<Result<List<MiniOrderInfoDTO>>> LazyGetProcessingOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData);
        Task<Result<List<HistoryOrderDTO>>> LazyGetCompletedOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData);
        Task<Result<int>> GetProcessingOrdersCountAsync();
        Task<Result<OrderHistoryDTO>> GetHistorySummaryAsync();
    }
}
