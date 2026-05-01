using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IPartServices
{
    public interface IRemovePartService
    {
        Task<Result> RemoveByIdAsync(Guid partId);
    }
}
