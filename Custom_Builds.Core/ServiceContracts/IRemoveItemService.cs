using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRemoveItemService
    {
        Task<Result> RemoveByIdAsync(Guid itemId);
    }
}
