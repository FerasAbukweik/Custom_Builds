using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IOrderServices
{
    public interface IGetOrderService
    {
        Task<Result<Order>> GetByIdAsync(Guid orderId);
    }
}
