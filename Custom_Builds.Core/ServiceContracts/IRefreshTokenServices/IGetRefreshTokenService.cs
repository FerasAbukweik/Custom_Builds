using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IRefreshTokenServices
{
    public interface IGetRefreshTokenService
    {
        Task<Result<RefreshToken>> GetFromRefreshTokenString(string refreshToken);
    }
}
