using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IRefreshTokenRepositry
    {
        Task AddRefrehTokenAsync(AddRefreshTokenDTO tokenInfo);
        Task<RefreshToken?> GetRefreshTokenFromRefreshTokenStringAsync(string refreshToken);
        Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(Guid userId);
        Task<ApplicationUser?> GetUserFromRefreshTokenStringAsync(string refreshTokenString);
        Task<RefreshToken?> GetRefreshTokenFromRefreshTokenIdAsync(Guid refreshTokenId);
        Task<bool> RemoveRefreshTokenByIdAsync(Guid tokenId);
        Task<bool> RemoveRefreshTokenByRefreshTokenStringAsync(string refreshToken);
        Task<bool> RemoveUserRefreshTokensAsync(Guid UserId);
    }
}
