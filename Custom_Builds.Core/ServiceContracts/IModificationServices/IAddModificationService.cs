using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ModificationServices
{
    public interface IAddModificationService
    {
        Task<Result<ModificationDTO>> AddAsync(AddModificationDTO toAdd);
    }
}
