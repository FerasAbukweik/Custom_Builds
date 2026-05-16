using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class GetCustomBuildService : IGetCustomBuildService
    {
        private readonly ICustomBuildRepository _customBuildRepository;
        private readonly IModificationsRepository _modificatoinsRepository;
        public GetCustomBuildService(ICustomBuildRepository customBuildRepository,
                                     IModificationsRepository modificatoinsRepository)
        {
            _customBuildRepository = customBuildRepository;
            _modificatoinsRepository = modificatoinsRepository;
        }
        public async Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId)
        {
            var result =  await _customBuildRepository.GetByIdAsync(customBuildId);
            if (!result.IsSuccess) return result.MapFailure<CustomBuild>();

            return Result<CustomBuild>.Success(result.Value!);
        }
        public async Task<Result<decimal>> GetTotalPriceAsync(Guid customBuildId)
        {
            // get custombuild repository so we can use it inside the contains
            var getCustomBuildEntityRes = await GetByIdAsync(customBuildId);
            if (!getCustomBuildEntityRes.IsSuccess) return getCustomBuildEntityRes.MapFailure<decimal>();

            var getModificationsResult = await _modificatoinsRepository.FilterAsync(m => m.CustomBuilds.Contains(getCustomBuildEntityRes.Value!));
            if(!getModificationsResult.IsSuccess) return getModificationsResult.MapFailure<decimal>();

            decimal totalPrice = 0m;

            foreach (var modification in getModificationsResult.Value!)
            {
                totalPrice += modification.Price;
            }

            return Result<decimal>.Success(totalPrice);
        }
    }
}