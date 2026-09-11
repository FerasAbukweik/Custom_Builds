using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Modification;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using HR_System.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class CachedModificationsService(
    [FromKeyedServices("inner")] IModificationsService innerService,
    IRedisService redisService,
    ILogger<CachedModificationsService> logger) : IModificationsService
{
    private const string ModificationCacheKeyPrefix = "Modifications";
    
    // Cascade invalidation prefixes
    private const string SectionCacheKeyPrefix = "Sections";
    private const string PartCacheKeyPrefix = "Parts";

    public async Task<Result<ModificationDTO>> AddAsync(ModificationAddDTO toAddModification, CancellationToken cancellationToken = default)
    {
        // 1. Execute the actual database/image operation via the inner service
        var result = await innerService.AddAsync(toAddModification, cancellationToken);

        // 2. Invalidate all relevant caches if the DB operation succeeded
        if (result.IsSuccess)
        {
            logger.LogInformation("Modification added successfully. Invalidating Modification, Section, and Part caches.");
            
            // Clear Modifications cache
            await redisService.RemoveByPrefixAsync(ModificationCacheKeyPrefix, cancellationToken);
            
            // Cascade Invalidation: clear parents and grandparents so top-level queries fetch fresh data
            await redisService.RemoveByPrefixAsync(SectionCacheKeyPrefix, cancellationToken);
            await redisService.RemoveByPrefixAsync(PartCacheKeyPrefix, cancellationToken);
        }

        return result;
    }

    public async Task<Result<decimal>> GetModificationsPriceAsync(IReadOnlyList<Guid> modificationIds, CancellationToken cancellationToken = default)
    {
        // Explicitly bypassing cache as requested. 
        // This passes the call directly to the inner DB service.
        return await innerService.GetModificationsPriceAsync(modificationIds, cancellationToken);
    }

    public async Task<Result<ModificationDTO>> RemoveByIdAsync(Guid modificationId, CancellationToken cancellationToken = default)
    {
        // 1. Execute the removal
        var result = await innerService.RemoveByIdAsync(modificationId, cancellationToken);

        // 2. Invalidate all relevant caches on success
        if (result.IsSuccess)
        {
            logger.LogInformation("Modification {ModificationId} removed successfully. Invalidating Modification, Section, and Part caches.", modificationId);
            
            // Clear Modifications cache
            await redisService.RemoveByPrefixAsync(ModificationCacheKeyPrefix, cancellationToken);
            
            // Cascade Invalidation
            await redisService.RemoveByPrefixAsync(SectionCacheKeyPrefix, cancellationToken);
            await redisService.RemoveByPrefixAsync(PartCacheKeyPrefix, cancellationToken);
        }

        return result;
    }
}