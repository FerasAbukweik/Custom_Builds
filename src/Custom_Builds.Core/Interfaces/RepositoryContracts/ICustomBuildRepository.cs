using System.Linq.Expressions;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.CustomBuild;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface ICustomBuildRepository
    {
        void Add(CustomBuild customBuildAdd);
        Task<CustomBuild?> GetByIdAsync(Guid customBuildId,
            Expression<Func<CustomBuild, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<CustomBuild?> EditByIdAsync(CustomBuildEditDTO newData, CancellationToken cancellationToken = default);
        Task<CustomBuild?> RemoveByIdAsync(Guid customBuildId, CancellationToken cancellationToken = default);
        Task<Result<List<CustomBuild>>> FilterAsync(Expression<Func<CustomBuild, bool>> extraChecks, Expression<Func<CustomBuild, object?>>[]? includes = null);
        Task<Result<decimal>> GetPriceAsync(Guid customBuildId, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
