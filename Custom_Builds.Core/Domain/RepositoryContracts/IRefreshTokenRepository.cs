using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IRefreshTokenRepository
    {
        Task<Result> AddAsync(AddRefreshTokenDTO tokenInfo);
        Task<Result<RefreshToken>> GetFromRefreshTokenStringAsync(string refreshToken);
        Task<Result<ApplicationUser>> GetUserFromRefreshTokenStringAsync(string refreshTokenString);
        Task<Result<RefreshToken>> GetFromIdAsync(Guid refreshTokenId);
        Task<Result> RemoveByIdAsync(Guid tokenId);
        Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken);
    }
}