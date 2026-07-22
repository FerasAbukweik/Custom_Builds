using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.CustomBuild;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface ICustomBuildService
{
    Task<Result<CustomBuildDTO>> AddCustomBuild(CustomBuildAddDTO toAddCustomBuild, Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<CustomBuildDTO>> GetByIdAsync(Guid customBuildId, Guid currUserId,
        CancellationToken cancellationToken = default);

    Task<Result<decimal>> GetPriceAsync(Guid customBuildId, CancellationToken cancellationToken = default);
    Task<Result<CustomBuildDTO>> RemoveByIdAsync(Guid customBuildId, CancellationToken cancellationToken = default);
}