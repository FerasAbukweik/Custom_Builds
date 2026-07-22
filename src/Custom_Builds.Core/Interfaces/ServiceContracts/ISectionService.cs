
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Section;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface ISectionService
{
    Task<Result<SectionDTO>> AddAsync(SectionAddDTO toAdd, CancellationToken cancellationToken = default);
    Task<Result<SectionDTO>> RemoveByIdAsync(Guid sectionId, CancellationToken cancellationToken = default);
}