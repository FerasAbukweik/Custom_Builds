using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IOrderServices
{
    public interface IRemoveOrderService
    {
        Task<Result> RemoveByIdAsync(Guid orderId);
    }
}
