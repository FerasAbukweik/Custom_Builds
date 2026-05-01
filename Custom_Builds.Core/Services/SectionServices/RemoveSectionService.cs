using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class RemoveSectionService : IRemoveSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public RemoveSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Result> RemoveByIdAsync(Guid sectionId)
        {
            return await _sectionRepository.RemoveByIdAsync(sectionId);
        }
    }
}
