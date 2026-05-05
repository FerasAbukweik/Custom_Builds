using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ISectionServices
{
    public interface IAddSectionService
    {
        Task<Result<SectionDTO>> AddAsync(AddSectionDTO toAdd);
    }
}
