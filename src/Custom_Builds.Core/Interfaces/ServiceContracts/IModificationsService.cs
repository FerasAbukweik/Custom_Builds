using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Modification;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IModificationsService
{
    Task<Result<ModificationDTO>> AddAsync(
        ModificationAddDTO toAddModification,
        CancellationToken cancellationToken = default);

    Task<Result<decimal>> GetModificationsPriceAsync(IReadOnlyList<Guid> modificationIds,
        CancellationToken cancellationToken = default);
    
    Task<Result<ModificationDTO>> RemoveByIdAsync(Guid modificationId, CancellationToken cancellationToken = default);
}