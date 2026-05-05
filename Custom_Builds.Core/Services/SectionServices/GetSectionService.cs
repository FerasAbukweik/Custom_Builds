using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
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

        public async Task<Result<SectionDTO>> GetByIdAsync(Guid sectionId)
        {
            var result = await _sectionRepository.GetByIdAsync(sectionId);
            if (!result.IsSuccess) return result.MapFailure<SectionDTO>();

            return Result<SectionDTO>.Success(result.Value!.toDTO());
        }
    }
}
