using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.DTO.Tokens;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IAuthService
{
    Task<Result> LoginAsync(LoginDTO loginInfo);
    void Logout(Guid userId);
    Task<Result<AccessAndRefreshTokenDTO>> UpdateTokensAsync(string refreshTokenString,
        CancellationToken cancellationToken = default);
}