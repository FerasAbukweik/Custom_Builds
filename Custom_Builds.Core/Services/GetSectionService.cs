using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class GetSectionService : IGetSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public GetSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<Result<Section>> GetByIdAsync(Guid sectionId)
        {
            return _sectionRepository.GetByIdAsync(sectionId);
        }
    }
}
