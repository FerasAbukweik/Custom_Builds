using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;

namespace Custom_Builds.Core.Services.ModificationServices
{
    public class RemoveModificationService : IRemoveModificationService
    {
        private readonly IModificationsRepository _modificationsRepository;

        public RemoveModificationService(IModificationsRepository modificationsRepository)
        {
            _modificationsRepository = modificationsRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid modificationId)
        {
            var result = await _modificationsRepository.RemoveByIdAsync(modificationId);

            return result;
        }
    }
}
