using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using System.Net;

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
        
        public async Task<Result<CustomBuildDTO>> AddByModificationsIdsAsync(AddCustomBuildDTO toAdd)
        {
            // get modifications by ids so we can add it to the custom build
            var modificationResult = await _modificationsRepository.GetListFromIdsAsync(toAdd.ModificationIds);
            if (!modificationResult.IsSuccess) return modificationResult.MapFailure<CustomBuildDTO>();

            // new customBuild
            CustomBuild customBuild = new CustomBuild
            {
                Id = Guid.NewGuid(),
                CustomBuildType = toAdd.CustomBuildType,
                Modifications = modificationResult.Value!,
                CreatorId = toAdd.CreatorId

            };

            // adding the custom build
            var addCustomBuildResult = await _customBuildRepository.AddEntityAsync(customBuild);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<CustomBuildDTO>();

            return Result<CustomBuildDTO>.Success(addCustomBuildResult.Value!.toDTO(), HttpStatusCode.Created);
        }
    }
}
