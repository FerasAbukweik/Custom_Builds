using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class RemoveCustomBuildService : IRemoveCustomBuildService
    {
        private readonly ICustomBuildRepository _customBuildRepository;
        public RemoveCustomBuildService(ICustomBuildRepository customBuildRepository)
        {
            _customBuildRepository = customBuildRepository;
        }
        public async Task<Result> RemoveByIdAsync(Guid customBuildId)
        {
            var result = await _customBuildRepository.RemoveByIdAsync(customBuildId);
            if (!result.IsSuccess) return result;

            return Result.Success();
        }
    }
}
