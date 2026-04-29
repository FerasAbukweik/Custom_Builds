using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRemoveOrderService
    {
        Task<Result> RemoveByIdAsync(Guid orderId);
    }
}
