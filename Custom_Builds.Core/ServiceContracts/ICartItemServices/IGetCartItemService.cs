using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICartItemServices
{
    public interface IGetCartItemService
    {
        Task<Result<CartItem>> GetItemByIdAsync(Guid cartItemId);
    }
}
