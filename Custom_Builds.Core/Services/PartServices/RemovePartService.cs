using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;

namespace Custom_Builds.Core.Services.PartServices
{
    public class RemovePartService : IRemovePartService
    {
        private readonly IPartRepository _partRepository;

        public RemovePartService(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid partId)
        {
            return await _partRepository.RemoveByIdAsync(partId);
        }
    }
}
