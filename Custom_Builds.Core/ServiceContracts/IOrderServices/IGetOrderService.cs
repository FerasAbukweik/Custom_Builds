using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IOrderServices
{
    public interface IGetOrderService
    {
        Task<Result<Order>> GetByIdAsync(Guid orderId);
        Task<Result<List<MiniOrderInfoDTO>>> GetProcessingOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData);
        Task<Result<List<HistoryOrderDTO>>> GetCompletedOrdersAsync(LazyGetUserDataDTO lazyGetUserOrdersData);
        Task<Result<int>> GetProcessingOrdersCountAsync(Guid? userId);
        Task<Result<OrderHistoryDTO>> GetHistorySummaryAsync(Guid? userId);
    }
}
