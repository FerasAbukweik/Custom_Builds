using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.CustomBuildServices
{
    public interface IAddCustomBuildService
    {
        Task<Result<Guid>> AddByModificationsIdsAsync(List<Guid> modificationIds, CustomBuildTypeEnum customBuildType);
    }
}
