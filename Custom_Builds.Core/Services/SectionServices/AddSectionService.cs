using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;
using Custom_Builds.Core.ServiceContracts.ISectionServices;

namespace Custom_Builds.Core.Services.SectionServices
{
    public class AddSectionService : IAddSectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IGetPartService _getPartSrevice;

        public AddSectionService(ISectionRepository sectionRepository,
                                 IGetPartService getPartSrevice)
        {
            _sectionRepository = sectionRepository;
            _getPartSrevice = getPartSrevice;
        }

        public async Task<Result<SectionDTO>> AddAsync(AddSectionDTO toAdd)
        {
            // get part which new section is attatched to
            var getPartResult = await _getPartSrevice.GetByIdAsync(toAdd.PartId);
            if (!getPartResult.IsSuccess) return getPartResult.MapFailure<SectionDTO>();

            // new Section
            Section newSection = new Section()
            {
                Id = Guid.NewGuid(),
                Title = toAdd.Title,
                Parts = [getPartResult.Value!]
            };

            // adding new section to the DB
            var result = await _sectionRepository.AddAsync(newSection);
            if (!result.IsSuccess) return result.MapFailure<SectionDTO>();

            return Result<SectionDTO>.Success(result.Value!.toDTO());
        }
    }
}
