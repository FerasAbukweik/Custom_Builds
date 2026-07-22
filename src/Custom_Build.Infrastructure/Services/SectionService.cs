using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Section;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class SectionService(
    ISectionRepository sectionRepository,
    ILogger<SectionService> logger) : ISectionService
{
    public async Task<Result<SectionDTO>> AddAsync(SectionAddDTO toAdd, CancellationToken cancellationToken = default)
    {
        // new Section
        Section newSection = new Section() 
        {
            Id = Guid.NewGuid(),
            Title = toAdd.Title,
            PartId = toAdd.PartId,
        };

        // adding new section to the DB
        sectionRepository.Add(newSection);

        if (!await sectionRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(SectionService), nameof(AddAsync));
            return Result<SectionDTO>.Failure("failed saving changes to DB");
        }

        return Result<SectionDTO>.Success(newSection.toDTO()); // no need to include modifications because there is none
    }
    public async Task<Result<SectionDTO>> RemoveByIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        var removed = await sectionRepository.RemoveByIdAsync(sectionId, cancellationToken);
        if (removed == null)
        {
            logger.LogError("{serviceName}.{methodName} section with id: {sectionId} was not found",
                nameof(SectionService), nameof(AddAsync), sectionId);
            return Result<SectionDTO>.Failure("sectio not found");
        }
        
        if (!await sectionRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(SectionService), nameof(RemoveByIdAsync));
            return Result<SectionDTO>.Failure("failed saving changes to DB");
        }

        return Result<SectionDTO>.Success(removed.toDTO()); // no need to include modifications because there is none
    }
}