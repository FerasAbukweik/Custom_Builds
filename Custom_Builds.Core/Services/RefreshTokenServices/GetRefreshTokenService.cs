using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;

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
            if (!result.IsSuccess) return result.MapFailure<RefreshToken>();

            return Result<RefreshToken>.Success(result.Value!);
        }
    }
}
