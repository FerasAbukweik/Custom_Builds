using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using System.Net;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class AddCustomBuildService : IAddCustomBuildService
    {
        private readonly IModificationsRepository _modificationsRepository;
        private readonly ICustomBuildRepository _customBuildRepository;

        public AddCustomBuildService(
            IModificationsRepository modificationsRepository,
            ICustomBuildRepository customBuildRepository)
        {
            _modificationsRepository = modificationsRepository;
            _customBuildRepository = customBuildRepository;
        }
        
        public async Task<Result<Guid>> AddByModificationsIdsAsync(List<Guid> modificationIds, CustomBuildTypeEnum customBuildType)
        {
            List<Modification> modifications = new List<Modification>();
            foreach (var modificationId in modificationIds)
            {
                var modificationResult = await _modificationsRepository.GetFromIdAsync(modificationId);
                if (!modificationResult.IsSuccess || modificationResult.Value == null)
                {
                    return Result<Guid>.Failure(modificationResult.ErrorMessage ?? "modification wasnt found", modificationResult.StatusCode);
                }

                modifications.Add(modificationResult.Value);
            }

            CustomBuild customBuild = new CustomBuild
            {
                Id = Guid.NewGuid(),
                CustomBuildType = customBuildType,
                Modifications = modifications
            };

            var addCustomBuildResult = await _customBuildRepository.AddEntityAsync(customBuild);
            if (!addCustomBuildResult.IsSuccess || addCustomBuildResult.Value == Guid.Empty)
            {
                return Result<Guid>.Failure(addCustomBuildResult.ErrorMessage ?? "failed to add custom build", addCustomBuildResult.StatusCode);
            }

            return Result<Guid>.Success(addCustomBuildResult.Value, HttpStatusCode.Created);
        }
    }
}
