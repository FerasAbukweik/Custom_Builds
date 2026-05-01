using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICustomBuildServices
{
    public interface IGetCustomBuildService
    {
        Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId);
    }
}
