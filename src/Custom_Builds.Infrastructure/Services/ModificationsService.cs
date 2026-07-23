using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Modification;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class ModificationsService(
    IModificationsRepository modificationsRepository,
    ILogger<ModificationsService> logger 
    ) : IModificationsService
{
    public async Task<Result<ModificationDTO>> AddAsync(
        ModificationAddDTO toAddModification,
        CancellationToken cancellationToken = default)
    {
        // new modification to add
        Modification newModification = new Modification()
        {
            Name = toAddModification.Name,
            Price = toAddModification.Price,
            Description = toAddModification.Description,
            Icon = toAddModification.Icon,
            Value = toAddModification.Value,
            SectionId = toAddModification.SectionId
        };

        // adding the modification to the DB
        modificationsRepository.Add(newModification);

        if (!await modificationsRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ModificationsService), nameof(AddAsync));
            return Result<ModificationDTO>.Failure("failed saving changes to DB");
        }

        return Result<ModificationDTO>.Success(newModification.toDTO());
    }

    public async Task<Result<decimal>> GetModificationsPriceAsync(IReadOnlyList<Guid> modificationIds, CancellationToken cancellationToken = default)
    {
        // get modifications count stored in the DB --used later to check if there is a missing modification
        var count = await modificationsRepository.CountAsync(m => modificationIds.Contains(m.Id), cancellationToken);
        
        // check if there is a missing modification
        if (count != modificationIds.Count)
        {
            logger.LogWarning("{serviceName}.{methodName} some modifications are missing",
                nameof(ModificationsService),  nameof(GetModificationsPriceAsync));
            return Result<decimal>.Failure("some modifications are missing");
        }
        
        return Result<decimal>.Success(await modificationsRepository.GetModificationsPriceAsync(modificationIds, cancellationToken));
    }
    
    public async Task<Result<ModificationDTO>> RemoveByIdAsync(Guid modificationId, CancellationToken cancellationToken = default)
    {
        var removed = await modificationsRepository.RemoveByIdAsync(modificationId, cancellationToken);

        if (removed == null)
        {
            logger.LogWarning("{serviceName}.{methodName} failed removing modification with id: {id} because it was not found",
                nameof(ModificationsService), nameof(RemoveByIdAsync), modificationId);
            return Result<ModificationDTO>.Failure("modification not found");
        }

        if (!await modificationsRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ModificationsService), nameof(RemoveByIdAsync));
            return Result<ModificationDTO>.Failure("failed saving changes to DB");
        }

        return Result<ModificationDTO>.Success(removed.toDTO());
    }
}