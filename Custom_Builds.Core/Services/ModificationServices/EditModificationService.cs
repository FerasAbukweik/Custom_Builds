using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;

namespace Custom_Builds.Core.Services.ModificationServices
{
    public class EditModificationService : IEditModificationService
    {
        private readonly IModificationsRepository _modificationsRepository;

        public EditModificationService(IModificationsRepository modificationsRepository)
        {
            _modificationsRepository = modificationsRepository;
        }

        public async Task<Result> EditByIdAsync(EditModificationDTO newData)
        {
            return await _modificationsRepository.EditByIdAsync(newData);
        }
    }
}
