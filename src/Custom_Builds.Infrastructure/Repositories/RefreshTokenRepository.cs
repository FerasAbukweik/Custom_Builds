using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Interfaces.RepositoryContracts;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
    {
        public void Add(RefreshToken newRefToken)
        {
            dbContext.RefreshTokens.Add(newRefToken);
        }
        public async Task<RefreshToken?> GetFromTokenStringAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.RefreshTokenString == refreshToken, cancellationToken);
        }
        public async Task<RefreshToken?> GetFromIdAsync(Guid refreshTokenId, CancellationToken cancellationToken = default)
        {
            return await dbContext.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(rt => rt.Id == refreshTokenId, cancellationToken);
        }
        public async Task<RefreshToken?> RemoveByIdAsync(Guid tokenId, CancellationToken cancellationToken = default)
        {
            var toDel = await GetFromIdAsync(tokenId, cancellationToken);

            if (toDel == null) return null;

            dbContext.RefreshTokens.Remove(toDel);

            return toDel;
        }
        public async Task<IReadOnlyList<RefreshToken>> FilterAsync(
            Expression<Func<RefreshToken, bool>> extraChecks,
            Expression<Func<RefreshToken, object?>>[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = dbContext.RefreshTokens.AsNoTracking().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(extraChecks).ToListAsync(cancellationToken);
        }
        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
