using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.CustomBuild;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class CustomBuildService(
    IModificationsRepository modificationsRepository,
    ICustomBuildRepository customBuildRepository,
    ILogger<CustomBuildService> logger) : ICustomBuildService
{
    public async Task<Result<CustomBuildDTO>> AddCustomBuild(CustomBuildAddDTO toAddCustomBuild,Guid userId, CancellationToken cancellationToken = default)
    {
        var modifications = await modificationsRepository
            .FilterAsync(m => toAddCustomBuild.ModificationIds.Contains(m.Id), [], cancellationToken);
        
        if(modifications.Count != toAddCustomBuild.ModificationIds.Count)
            return Result<CustomBuildDTO>.Failure("some modifications where not found");

        var newCustomBuild = new CustomBuild()
        {
            UserId = userId,
            Modifications = modifications.ToList(),
            CustomBuildType = toAddCustomBuild.CustomBuildType
        };
        
        customBuildRepository.Add(newCustomBuild);

        if (!await customBuildRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{modificationName} failed saving changes to DB",
                nameof(CustomBuildService), nameof(AddCustomBuild));
            return Result<CustomBuildDTO>.Failure("failed saving changes to DB");
        }

        return Result<CustomBuildDTO>.Success(newCustomBuild.toDTO());
    }
    public async Task<Result<CustomBuildDTO>> GetByIdAsync(Guid customBuildId,Guid currUserId, CancellationToken cancellationToken  = default)
    {
        var customBuild =  await customBuildRepository.GetByIdAsync(customBuildId,[], cancellationToken);
        if(customBuild == null)
            return Result<CustomBuildDTO>.Failure("Custom Build was not found");
        
        if(customBuild.UserId != currUserId)
            return Result<CustomBuildDTO>.Failure("Unauthorized", HttpStatusCode.Unauthorized);

        return Result<CustomBuildDTO>.Success(customBuild.toDTO());
    }
    public async Task<Result<decimal>> GetPriceAsync(Guid customBuildId, CancellationToken cancellationToken = default)
    {
        var result = await customBuildRepository.GetPriceAsync(customBuildId, cancellationToken);
        if (!result.IsSuccess) return result.MapFailure<decimal>();

        return Result<decimal>.Success(result.Value!);
    }
    public async Task<Result<CustomBuildDTO>> RemoveByIdAsync(Guid customBuildId, CancellationToken cancellationToken = default)
    {
        var toRemove = await customBuildRepository.RemoveByIdAsync(customBuildId, cancellationToken);
        if (toRemove == null)
        {
            logger.LogError("{serviceName}.{methodName} custom build with id: {customBuildId} was not found",
                nameof(CustomBuildService), nameof(RemoveByIdAsync), customBuildId);
            return Result<CustomBuildDTO>.Failure("custom build not found");
        }

        if (!await customBuildRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(CustomBuildService), nameof(RemoveByIdAsync));
            return Result<CustomBuildDTO>.Failure("failed saving changes to DB");
        }
        
        return Result<CustomBuildDTO>.Success(toRemove.toDTO());
    }
}