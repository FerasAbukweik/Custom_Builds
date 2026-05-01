using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IModificationServices
{
    public interface IEditModificationService
    {
        Task<Result> EditByIdAsync(EditModificationDTO newData);
    }
}
