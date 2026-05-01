using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICartItemServices
{
    public interface IEditCartItemService
    {
        Task<Result> EditByIdAsync(EditCartItemDTO newData);
    }
}
