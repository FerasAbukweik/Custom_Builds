using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Section;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface ISectionRepository
    {
        Task<Section?> GetByIdAsync(
            Guid sectionId,
            Expression<Func<Section, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        void Add(Section toAdd);
        Task<Section?> RemoveByIdAsync(Guid sectionId, CancellationToken cancellationToken = default);
        Task<Section?> EditByIdAsync(SectionEditDTO newData, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Section>> FilterAsync(
            Expression<Func<Section, bool>> extraChecks,
            Expression<Func<Section, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
