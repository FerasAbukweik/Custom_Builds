using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICustomBuildServices
{
    public interface IEditCustomBuildService { Task<Result> EditByIdAsync(EditCustomBuildDTO newData); }
}
