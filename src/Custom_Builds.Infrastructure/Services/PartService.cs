using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Part;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class PartService(
    IPartRepository partRepository,
    ILogger<PartService> logger) : IPartService
{
    public async Task<Result<PartDTO>> AddAsync(PartAddDTO toPartAdd, CancellationToken cancellationToken = default)
    {
        // new part
        Part newPart = new Part()
        {
            Id = Guid.NewGuid(),
            Name = toPartAdd.Name,
            Icon = toPartAdd.Icon,
        };

        partRepository.Add(newPart);

        if (!await partRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(PartService), nameof(AddAsync));
            return Result<PartDTO>.Failure("Failed saving changes to DB");
        }

        return Result<PartDTO>.Success(newPart.toDTO()); // no need to include sections since there is none
    }
    public async Task<Result<IReadOnlyList<PartDTO>>> GetAllPartsIncludingAllData(CancellationToken cancellationToken = default)
    {
        var result = await partRepository.GetAllAsync(cancellationToken);

        return Result<IReadOnlyList<PartDTO>>.Success(result.Select(r => r.toDTO()).ToList());
    }
    public async Task<Result<PartDTO>> RemoveByIdAsync(Guid partId, CancellationToken cancellationToken = default)
    {
       var removed = await partRepository.RemoveByIdAsync(partId, cancellationToken);
       if (removed == null)
       {
           logger.LogWarning("{serviceName}.{methodName} part with id: {partId} was not found",
               nameof(PartService), nameof(RemoveByIdAsync), partId);
           return Result<PartDTO>.Failure("part was not found");
       }
       
       if (!await partRepository.SaveChangesAsync(cancellationToken))
       {
           logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
               nameof(PartService), nameof(RemoveByIdAsync));
           return Result<PartDTO>.Failure("Failed saving changes to DB");
       }
       
       return Result<PartDTO>.Success(removed.toDTO());
    }
}