using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ICustomBuildRepository
    {
        Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId);
        Task<Result<Guid>> AddAsync(AddCustomBuildDTO toAdd);
        Task<Result<Guid>> AddEntityAsync(CustomBuild customBuild);
        Task<Result> EditByIdAsync(EditCustomBuildDTO newData);
        Task<Result> RemoveByIdAsync(Guid customBuildId);
    }
}
