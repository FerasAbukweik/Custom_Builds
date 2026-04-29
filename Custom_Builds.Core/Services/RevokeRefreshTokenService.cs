using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RevokeRefreshTokenService : IRevokeRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeRefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task RemoveByRefreshTokenStringAsync(string refreshToken)
        {
            await _refreshTokenRepository.RemoveByRefreshTokenStringAsync(refreshToken);
        }
    }
}
