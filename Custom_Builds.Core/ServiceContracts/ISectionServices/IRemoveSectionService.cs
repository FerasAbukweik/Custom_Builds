using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ISectionServices
{
    public interface IRemoveSectionService
    {
        Task<Result> RemoveByIdAsync(Guid sectionId);
    }
}
