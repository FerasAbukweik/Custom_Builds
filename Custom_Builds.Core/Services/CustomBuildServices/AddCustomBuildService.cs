using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using System.Net;
using System.Runtime.CompilerServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class AddCustomBuildService : IAddCustomBuildService
    {
        private readonly IModificationsRepository _modificationsRepository;
        private readonly ICustomBuildRepository _customBuildRepository;
        private readonly IGetCurrUserService _getCurrUserService;

        public AddCustomBuildService(
            IModificationsRepository modificationsRepository,
            ICustomBuildRepository customBuildRepository,
            IGetCurrUserService getCurrUserService)
        {
            _modificationsRepository = modificationsRepository;
            _customBuildRepository = customBuildRepository;
            _getCurrUserService = getCurrUserService;
        }
        
        public async Task<Result<CustomBuildDTO>> AddByModificationsIdsAsync(AddCustomBuildDTO toAdd)
        {
            // hashset for faster comparision
            HashSet<Guid> idsSet = new HashSet<Guid>(toAdd.ModificationIds);

            // get modifications by ids so we can add it to the custom build
            var getModificationResult = await _modificationsRepository.FilterAsync(m => idsSet.Contains(m.Id));
            if (!getModificationResult.IsSuccess) return getModificationResult.MapFailure<CustomBuildDTO>();

            // get curr user id
            var getCurrUserIdResult = _getCurrUserService.GetUserId();
            if (!getCurrUserIdResult.IsSuccess) return getCurrUserIdResult.MapFailure<CustomBuildDTO>();

            // new customBuild
            CustomBuild customBuild = new CustomBuild
            {
                Id = Guid.NewGuid(),
                CustomBuildType = toAdd.CustomBuildType,
                Modifications = getModificationResult.Value!,
                CreatorId = getCurrUserIdResult.Value!
            };

            // adding the custom build to DB
            var addCustomBuildResult = await _customBuildRepository.AddEntityAsync(customBuild);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<CustomBuildDTO>();

            return Result<CustomBuildDTO>.Success(addCustomBuildResult.Value!.toDTO(), HttpStatusCode.Created);
        }
    }
}
