using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.PartServices
{
    public interface IAddPartService
    {
        Task<Result<Guid>> AddAsync(AddPartDTO toAdd);
    }
}
