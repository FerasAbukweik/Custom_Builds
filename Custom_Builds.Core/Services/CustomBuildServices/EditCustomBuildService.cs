using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICustomBuildServices;

namespace Custom_Builds.Core.Services.CustomBuildServices
{
    public class EditCustomBuildService : IEditCustomBuildService
    {
        private readonly ICustomBuildRepository _customBuildRepository;
        public EditCustomBuildService(ICustomBuildRepository customBuildRepository) => _customBuildRepository = customBuildRepository;
        public Task<Result> EditByIdAsync(EditCustomBuildDTO newData) => _customBuildRepository.EditByIdAsync(newData);
    }
}
