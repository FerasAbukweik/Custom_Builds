using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Section;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using HR_System.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class CachedSectionService(
    [FromKeyedServices("inner")] ISectionService innerService,
    IRedisService redisService,
    ILogger<CachedSectionService> logger) : ISectionService
{
    private const string SectionCacheKeyPrefix = "Sections";
    
    // We need to invalidate Parts because GetAllPartsIncludingAllData likely includes Sections
    private const string PartCacheKeyPrefix = "Parts"; 

    public async Task<Result<SectionDTO>> AddAsync(SectionAddDTO toAdd, CancellationToken cancellationToken = default)
    {
        // 1. Execute the actual database operation via the inner service
        var result = await innerService.AddAsync(toAdd, cancellationToken);

        // 2. Invalidate caches if the DB operation succeeded
        if (result.IsSuccess)
        {
            logger.LogInformation("Section added successfully. Invalidating Section and Part caches.");
            
            // Invalidate Sections
            await redisService.RemoveByPrefixAsync(SectionCacheKeyPrefix, cancellationToken);
            
            // Cascade Invalidation: A new section means the parent Part has changed
            await redisService.RemoveByPrefixAsync(PartCacheKeyPrefix, cancellationToken);
        }

        return result;
    }

    public async Task<Result<SectionDTO>> RemoveByIdAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        // 1. Execute the removal
        var result = await innerService.RemoveByIdAsync(sectionId, cancellationToken);

        // 2. Invalidate caches on success
        if (result.IsSuccess)
        {
            logger.LogInformation("Section {SectionId} removed successfully. Invalidating Section and Part caches.", sectionId);
            
            // Invalidate Sections
            await redisService.RemoveByPrefixAsync(SectionCacheKeyPrefix, cancellationToken);
            
            // Cascade Invalidation: Removing a section means the parent Part has changed
            await redisService.RemoveByPrefixAsync(PartCacheKeyPrefix, cancellationToken);
        }

        return result;
    }
}