using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.CartItemServices
{
    public interface IAddCartItemService
    {
        Task<Result<Guid>> AddAsync(AddCartItemDTO toAdd);
        Task<Result<Guid>> AddCustomBuildAsync(AddCustomBuildDTO toAdd);
    }
}
