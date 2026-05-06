using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;

namespace Custom_Builds.Core.Services.PartServices
{
    public class EditPartService : IEditPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly ISectionRepository _sectionRepository;

        public EditPartService(IPartRepository partRepository,
                               ISectionRepository sectionRepository)
        {
            _partRepository = partRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<Result> EditByIdAsync(EditPartDTO newData)
        {
            var result = await _partRepository.EditByIdAsync(newData);

            return result;
        }

        public async Task<Result> LinkSectionAsync(LinkSectionDTO linkData)
        {
            // get section object so we can link it with the part
            var getSectionToLink = await _sectionRepository.GetByIdAsync(linkData.SectionId);
            if (!getSectionToLink.IsSuccess) return getSectionToLink;

            // trying to add section to part
            var linkResult = await _partRepository.LinkSectionAsync(linkData.PartId, getSectionToLink.Value!);

            return linkResult;
        }
    }
}
