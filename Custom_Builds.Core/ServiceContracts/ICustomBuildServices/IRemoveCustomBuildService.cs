using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICustomBuildServices
{
    public interface IRemoveCustomBuildService
    {
        Task<Result> RemoveByIdAsync(Guid customBuildId);
    }
}
