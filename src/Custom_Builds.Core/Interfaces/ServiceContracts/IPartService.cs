using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Part;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IPartService
{
    Task<Result<PartDTO>> AddAsync(PartAddDTO toPartAdd, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PartDTO>>> GetAllPartsIncludingAllData(CancellationToken cancellationToken = default);
    Task<Result<PartDTO>> RemoveByIdAsync(Guid partId, CancellationToken cancellationToken = default);
}