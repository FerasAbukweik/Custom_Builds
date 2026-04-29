using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRemoveCartService
    {
        Task<Result> RemoveByIdAsync(Guid cartId);
    }
}
