using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class AddSectionService : IAddSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public AddSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Result<Guid>> AddAsync(AddSectionDTO toAdd)
        {
            return await _sectionRepository.AddAsync(toAdd);
        }
    }
}
