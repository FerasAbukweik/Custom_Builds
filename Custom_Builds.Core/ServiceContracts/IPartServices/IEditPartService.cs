using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IPartServices
{
    public interface IEditPartService
    {
        Task<Result> EditByIdAsync(EditPartDTO newData);
        Task<Result> LinkSectionAsync(LinkSectionDTO linkData);
    }
}
