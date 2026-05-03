using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IRefreshTokenRepository
    {
        Task<Result<RefreshToken>> AddAsync(AddRefreshTokenDTO tokenInfo);
        Task<Result<RefreshToken>> GetFromRefreshTokenStringAsync(string refreshToken);
        Task<Result<RefreshToken>> GetFromIdAsync(Guid refreshTokenId);
        Task<Result> RemoveByIdAsync(Guid tokenId);
        Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken);
    }
}