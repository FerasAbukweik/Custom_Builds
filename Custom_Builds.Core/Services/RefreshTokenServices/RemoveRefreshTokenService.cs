using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;

namespace Custom_Builds.Core.Services.RefreshTokenServices
{
    public class RemoveRefreshTokenService : IRemoveRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RemoveRefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken)
        {
            return await _refreshTokenRepository.RemoveByRefreshTokenStringAsync(refreshToken);
        }
    }
}
