using System.Linq.Expressions;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;

namespace Custom_Builds.Core.Interfaces.RepositoryContracts
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken newRefToken);
        Task<RefreshToken?> GetFromTokenStringAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetFromIdAsync(Guid refreshTokenId, CancellationToken cancellationToken = default);
        Task<RefreshToken?> RemoveByIdAsync(Guid tokenId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RefreshToken>> FilterAsync(
            Expression<Func<RefreshToken, bool>> extraChecks,
            Expression<Func<RefreshToken, object?>>[]? includes = null,
            CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}