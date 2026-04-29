using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class AddSectionService : IAddSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public AddSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<Result> AddAsync(AddSectionDTO toAdd)
        {
            return _sectionRepository.AddAsync(toAdd);
        }
    }
}
