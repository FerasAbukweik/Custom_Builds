using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class EditSectionService : IEditSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public EditSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public Task<Result> EditByIdAsync(EditSectionDTO newData)
        {
            return _sectionRepository.EditByIdAsync(newData);
        }
    }
}
