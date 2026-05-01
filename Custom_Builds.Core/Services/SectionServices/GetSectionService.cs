using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class GetSectionService : IGetSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public GetSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Result<Section>> GetByIdAsync(Guid sectionId)
        {
            return await _sectionRepository.GetByIdAsync(sectionId);
        }
    }
}
