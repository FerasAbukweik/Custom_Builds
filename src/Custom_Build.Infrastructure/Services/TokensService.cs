using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Tokens;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Infrastructure.Services;

public class TokensService(
    ICookieService cookieService,
    IRefreshTokenRepository refreshTokenRepository,
    IAccessTokenService accessTokenService,
    IRefreshTokenService refreshTokenService,
    UserManager<ApplicationUser> userManager) : ITokensService 
{
    public async Task<Result<AccessAndRefreshTokenDTO>> UpdateTokensAsync(string refreshTokenString, CancellationToken cancellationToken = default)
    {
        // get refresh token from DB
        var refreshToken =
            await refreshTokenRepository.GetFromTokenStringAsync(refreshTokenString, cancellationToken);
        if(refreshToken is null || refreshToken.IsExpired) return Result<AccessAndRefreshTokenDTO>.Failure("Bad refresh token");
        
        // get Application User
        var user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if(user == null) return  Result<AccessAndRefreshTokenDTO>.Failure("bad refresh token");

        
        var generateTokensResult = await GenerateTokens(user);
        if(!generateTokensResult.IsSuccess) return generateTokensResult;
        
        // add tokens to response cookies
        var addTokensResult = cookieService.SetTokens(generateTokensResult.Value!);
        if (!addTokensResult.IsSuccess) return addTokensResult.MapFailure<AccessAndRefreshTokenDTO>();

        // return new tokens
        return Result<AccessAndRefreshTokenDTO>.Success(generateTokensResult.Value!);
    }

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