using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ICustomBuildRepository
    {
        Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId);
        Task<Result<CustomBuild>> AddAsync(AddCustomBuildDTOToDB toAdd);
        Task<Result<CustomBuild>> AddEntityAsync(CustomBuild toAdd);
        Task<Result> EditByIdAsync(EditCustomBuildDTO newData);
        Task<Result> RemoveByIdAsync(Guid customBuildId);
    }
}
