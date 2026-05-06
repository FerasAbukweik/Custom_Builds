using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class EditSectionService : IEditSectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IGetModificationService _getModificationService;

        public EditSectionService(ISectionRepository sectionRepository,
                                  IGetModificationService getModificationService)
        {
            _sectionRepository = sectionRepository;
            _getModificationService = getModificationService;
        }

        public async Task<Result> EditByIdAsync(EditSectionDTO newData)
        {
            var result = await _sectionRepository.EditByIdAsync(newData);
            if (!result.IsSuccess) return result.MapFailure();

            return Result.Success();
        }

        public async Task<Result> LinkModification(LinkModificationDTO linkData)
        {
            // get modificatio object so we can add it to Section
            var getModificationResult = await _getModificationService.GetFromIdAsync(linkData.modificationId);
            if (!getModificationResult.IsSuccess) return getModificationResult;

            var linkModificationResult = await _sectionRepository.AddModificationAsync(linkData.sectionId, getModificationResult.Value!);

            return linkModificationResult;
        }
    }
}
