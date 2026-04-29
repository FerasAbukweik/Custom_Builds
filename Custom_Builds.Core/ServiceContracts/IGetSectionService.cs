using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IGetSectionService
    {
        Task<Result<Section>> GetByIdAsync(Guid sectionId);
    }
}
