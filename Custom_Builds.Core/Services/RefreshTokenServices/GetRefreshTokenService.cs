using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Services.RefreshTokenServices
{
    public class GetRefreshTokenService : IGetRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public GetRefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<RefreshToken>> GetFromRefreshTokenString(string refreshToken)
        {
            var result = await _refreshTokenRepository.GetFromRefreshTokenStringAsync(refreshToken);
            if (!result.IsSuccess)
            {
                return Result<RefreshToken>.Failure(result.ErrorMessage ?? "Failed To Get Refresh Token" , result.StatusCode);
            }

            return Result<RefreshToken>.Success(result.Value!);
        }
    }
}
