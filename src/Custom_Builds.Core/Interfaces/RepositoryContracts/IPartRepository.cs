using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Part;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IPartRepository
    {
        void Add(Part toAdd);
        Task<Part?> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default);
        Task<Part?> RemoveByIdAsync(Guid partId, CancellationToken cancellationToken = default);
        Task<Part?> EditByIdAsync(PartEditDTO newData, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Part>> FilterAsync(
            Expression<Func<Part, bool>> extraChecks,
            Expression<Func<Part, object>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Part>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
