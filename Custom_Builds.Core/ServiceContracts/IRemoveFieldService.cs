using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRemoveFieldService
    {
        Task<Result> RemoveByIdAsync(Guid fieldId);
    }
}
