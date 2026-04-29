using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RemoveSectionService : IRemoveSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public RemoveSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<Result> RemoveByIdAsync(Guid sectionId)
        {
            return _sectionRepository.RemoveByIdAsync(sectionId);
        }
    }
}
