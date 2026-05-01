using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ModificationServices;

namespace Custom_Builds.Core.Services.ModificationServices
{
    public class AddModificationService : IAddModificationService
    {
        private readonly IModificationsRepository _modificationsRepository;

        public AddModificationService(IModificationsRepository modificationsRepository)
        {
            _modificationsRepository = modificationsRepository;
        }

        public async Task<Result<Guid>> AddAsync(AddModificationDTO toAdd)
        {
            return await _modificationsRepository.AddAsync(toAdd);
        }
    }
}
