using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class EditSectionService : IEditSectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public EditSectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Result> EditByIdAsync(EditSectionDTO newData)
        {
            var result = await _sectionRepository.EditByIdAsync(newData);
            if (!result.IsSuccess) return result.MapFailure();

            return Result.Success();
        }
    }
}
