using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class GetCustomBuildService : IGetCustomBuildService
    {
        private readonly ICustomBuildRepository _customBuildRepository;
        public GetCustomBuildService(ICustomBuildRepository customBuildRepository)
        {
            _customBuildRepository = customBuildRepository;
        }
        public async Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId)
        {
            var result =  await _customBuildRepository.GetByIdAsync(customBuildId);
            if (!result.IsSuccess) return result.MapFailure<CustomBuild>();

            return Result<CustomBuild>.Success(result.Value!);
        }


    }
}
