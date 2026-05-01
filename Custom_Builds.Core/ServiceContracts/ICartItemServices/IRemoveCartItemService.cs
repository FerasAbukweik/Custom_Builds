using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICartItemServices
{
    public interface IRemoveCartItemService
    {
        Task<Result> RemoveByIdAsync(Guid cartItemId);
    }
}
