using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Linq.Expressions;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IRefreshTokenRepository
    {
        Task<Result<RefreshToken>> AddAsync(RefreshToken tokenInfo);
        Task<Result<RefreshToken>> GetFromRefreshTokenStringAsync(string refreshToken);
        Task<Result<RefreshToken>> GetFromIdAsync(Guid refreshTokenId);
        Task<Result> RemoveByIdAsync(Guid tokenId);
        Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken);
        Task<Result<List<RefreshToken>>> FilterAsync(Expression<Func<RefreshToken, bool>> extraChecks, Expression<Func<RefreshToken, object>>[]? includes = null);
    }
}