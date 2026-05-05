using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ISectionServices
{
    public interface IGetSectionService
    {
        Task<Result<SectionDTO>> GetByIdAsync(Guid sectionId);
    }
}
