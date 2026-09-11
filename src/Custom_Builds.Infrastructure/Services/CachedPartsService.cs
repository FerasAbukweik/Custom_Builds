using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Part;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using HR_System.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class CachedPartService(
    [FromKeyedServices("inner")] IPartService innerService,
    IRedisService redisService,
    ILogger<CachedPartService> logger) : IPartService
{
    private const string CacheKeyPrefix = "Parts";
    private const string AllPartsCacheKey = $"{CacheKeyPrefix}:All";

    public async Task<Result<PartDTO>> AddAsync(PartAddDTO toPartAdd, CancellationToken cancellationToken = default)
    {
        // 1. Execute the actual database operation via the inner service
        var result = await innerService.AddAsync(toPartAdd, cancellationToken);
        
        // 2. Invalidate cache ONLY if the DB operation succeeded
        if (result.IsSuccess) 
        {
            logger.LogInformation("Part added successfully. Invalidating cache with prefix: {Prefix}", CacheKeyPrefix);
            await redisService.RemoveByPrefixAsync(CacheKeyPrefix, cancellationToken);
        }

        return result;
    }

    public async Task<Result<IReadOnlyList<PartDTO>>> GetAllPartsIncludingAllData(CancellationToken cancellationToken = default)
    {
        // 1. Try fetching the raw list from the cache
        var cachedParts = await redisService.GetAsync<IReadOnlyList<PartDTO>>(AllPartsCacheKey, cancellationToken);
        
        if (cachedParts is not null)
        {
            logger.LogInformation("Fetched parts from Redis cache.");
            // Re-wrap the cached list in your Result object
            return Result<IReadOnlyList<PartDTO>>.Success(cachedParts);
        }

        // 2. Cache miss: fetch from database
        logger.LogInformation("Cache miss. Fetching parts from database.");
        var result = await innerService.GetAllPartsIncludingAllData(cancellationToken);

        // 3. Set cache if the DB fetch was successful
        // Note: Change 'result.Value' to whatever property your Result<T> uses (e.g., result.Data)
        if (result.IsSuccess && result.Value is not null)
        {
            await redisService.SetAsync(AllPartsCacheKey, result.Value, cancellationToken);
        }

        return result;
    }

    public async Task<Result<PartDTO>> RemoveByIdAsync(Guid partId, CancellationToken cancellationToken = default)
    {
        // 1. Execute the removal
        var result = await innerService.RemoveByIdAsync(partId, cancellationToken);

        // 2. Invalidate cache on success
        if (result.IsSuccess)
        {
            logger.LogInformation("Part {PartId} removed successfully. Invalidating cache.", partId);
            await redisService.RemoveByPrefixAsync(CacheKeyPrefix, cancellationToken);
        }

        return result;
    }
}