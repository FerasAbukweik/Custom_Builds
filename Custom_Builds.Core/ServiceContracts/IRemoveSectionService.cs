using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IRemoveSectionService
    {
        Task<Result> RemoveByIdAsync(Guid sectionId);
    }
}
