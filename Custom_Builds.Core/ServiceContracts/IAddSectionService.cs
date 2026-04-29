using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IAddSectionService
    {
        Task<Result> AddAsync(AddSectionDTO toAdd);
    }
}
