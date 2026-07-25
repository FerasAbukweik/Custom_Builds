using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Tokens;
using Custom_Builds.Core.Interfaces.ServiceContracts;

namespace Custom_Builds.Infrastructure.Services;

public class TokensService(
    IAccessTokenService accessTokenService,
    IRefreshTokenService refreshTokenService) : ITokensService 
{
    public async Task<Result<AccessAndRefreshTokenDTO>> GenerateTokens(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        // generate access token
        var accessTokenResult = await accessTokenService.GenerateAccessTokenAsync(user);
        if (!accessTokenResult.IsSuccess) return accessTokenResult.MapFailure<AccessAndRefreshTokenDTO>();

        // generate refresh token
        var refreshTokenResult = await refreshTokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
        if (!refreshTokenResult.IsSuccess) return refreshTokenResult.MapFailure<AccessAndRefreshTokenDTO>();

        return Result<AccessAndRefreshTokenDTO>.Success(new AccessAndRefreshTokenDTO()
        {
            AccessToken = accessTokenResult.Value!,
            RefreshToken = refreshTokenResult.Value!.RefreshTokenString
        });
    }
    
}