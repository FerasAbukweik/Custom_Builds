using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Modification;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IModificationsRepository
    {
        Task<Modification?> GetByIdAsync(
            Guid modificationId,
            Expression<Func<Modification, object?>>[]? includes,
            CancellationToken cancellationToken = default);
        void Add(Modification toAdd);
        Task<Modification?> EditByIdAsync(ModificationEditDTO newData, CancellationToken cancellationToken = default);
        Task<Modification?> RemoveByIdAsync(Guid modificationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Modification>> FilterAsync(Expression<Func<Modification, bool>> extraChecks,
            Expression<Func<Modification, object?>>[]? includes,
            CancellationToken cancellationToken = default);
        
        Task<int> CountAsync(Expression<Func<Modification, bool>> extraChecks, CancellationToken cancellationToken = default);

        Task<decimal> GetModificationsPriceAsync(IReadOnlyList<Guid> modificationIds, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
