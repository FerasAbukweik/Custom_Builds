using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IEditItemService
    {
        Task<Result> EditByIdAsync(EditItemDTO newData);
    }
}
