using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.CartItemServices
{
    public interface IAddCartItemService
    {
        Task<Result<CartItemDTO>> AddAsync(Guid productId);
        Task<Result<CartItemDTO>> AddCustomBuildAsync(AddCustomBuildDTO toAdd);
    }
}
