using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;

namespace Custom_Builds.Infrastructure.Repositories;

public class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public async Task<IReadOnlyList<ApplicationUser>> FilterAsync(
        Expression<Func<ApplicationUser, bool>> filter,
        Expression<Func<ApplicationUser, object?>>[]? includes = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users.AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
        }

        return await query.Where(filter).ToListAsync(cancellationToken);
    }
}