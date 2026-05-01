using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class RemoveCustomBuildService : IRemoveCustomBuildService
    {
        private readonly ICustomBuildRepository _customBuildRepository;
        public RemoveCustomBuildService(ICustomBuildRepository customBuildRepository) => _customBuildRepository = customBuildRepository;
        public Task<Result> RemoveByIdAsync(Guid customBuildId) => _customBuildRepository.RemoveByIdAsync(customBuildId);
    }
}
