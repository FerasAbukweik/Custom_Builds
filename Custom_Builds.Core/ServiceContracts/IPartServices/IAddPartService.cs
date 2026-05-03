using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.PartServices
{
    public interface IAddPartService
    {
        Task<Result<PartDTO>> AddAsync(AddPartDTO toAdd);
    }
}
