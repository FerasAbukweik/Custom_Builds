using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Identity;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts;

public interface IUsersRepository
{
    Task<IReadOnlyList<ApplicationUser>> FilterAsync(
        Expression<Func<ApplicationUser, bool>> filter,
        Expression<Func<ApplicationUser, object?>>[]? includes = null,
        CancellationToken cancellationToken = default);
}