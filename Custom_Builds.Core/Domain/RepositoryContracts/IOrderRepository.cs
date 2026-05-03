using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IOrderRepository
    {
        Task<Result<Order>> GetByIdAsync(Guid orderId);
        Task<Result<Order>> AddAsync(AddOrderTODB toAdd);
        Task<Result> EditByIdAsync(EditOrderDTO newData);
        Task<Result> RemoveByIdAsync(Guid orderId);
        Task<Result<List<MiniOrderInfoDTO>>> GetOrdersByUserIdAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData);
        Task<Result<List<MiniOrderInfoDTO>>> GetCompletedUserOrdersAsync(LazyGetALlOrdersDTO lazyGetUserOrdersData);
    }
}
