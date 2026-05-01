using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IModificationServices
{
    public interface IRemoveModificationService
    {
        Task<Result> RemoveByIdAsync(Guid modificationId);
    }
}
