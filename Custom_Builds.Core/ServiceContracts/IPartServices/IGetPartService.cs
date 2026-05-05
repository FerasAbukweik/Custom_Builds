using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IPartServices
{
    public interface IGetPartService
    {
        Task<Result<Part>> GetByIdAsync(Guid partId);
        Task<Result<List<Part>>> GetAllPartsIncludingAllData();
    }
}
