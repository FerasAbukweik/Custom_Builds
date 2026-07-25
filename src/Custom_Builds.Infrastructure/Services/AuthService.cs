using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.DTO.Tokens;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    ILogger<AuthService> logger,
    ITokensService tokensService,
    ICookieService cookieService,
    IRefreshTokenRepository refreshTokenRepository
    ) : IAuthService
{
    public async Task<Result> LoginAsync(LoginDTO loginInfo) 
    {
        // find user by email
        ApplicationUser? user = await userManager.FindByEmailAsync(loginInfo.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginInfo.Password))
        {
            logger.LogWarning("{serviceName}.{methodName} failed login attempt for email: {email}",
                nameof(AccountService), nameof(LoginAsync), loginInfo.Email);
            return Result.Failure("Wrong Email or Password" , HttpStatusCode.Unauthorized);
        }

        // generate Tokens
        var generateTokensResult = await tokensService.GenerateTokens(user);
        if (!generateTokensResult.IsSuccess) return generateTokensResult;
        
        // store tokens in cookies response
        var storeTokensResult = cookieService.SetTokens(generateTokensResult.Value!);
        if (!storeTokensResult.IsSuccess) return storeTokensResult;

        logger.LogInformation("{serviceName}.{methodName} user with id: {userId} logged in",
            nameof(AccountService), nameof(LoginAsync), user.Id);
        
        return Result.Success();
    }

    public async Task<Result<AccessAndRefreshTokenDTO>> UpdateTokensAsync(string refreshTokenString, CancellationToken cancellationToken = default)
    {
        // get refresh token from DB
        var refreshToken =
            await refreshTokenRepository.GetFromTokenStringAsync(refreshTokenString, cancellationToken);
        if(refreshToken is null || refreshToken.IsExpired) return Result<AccessAndRefreshTokenDTO>.Failure("Bad refresh token");
        
        // get Application User
        var user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if(user == null) return  Result<AccessAndRefreshTokenDTO>.Failure("bad refresh token");

        
        var generateTokensResult = await tokensService.GenerateTokens(user, cancellationToken);
        if(!generateTokensResult.IsSuccess) return generateTokensResult;
        
        // add tokens to response cookies
        var addTokensResult = cookieService.SetTokens(generateTokensResult.Value!);
        if (!addTokensResult.IsSuccess) return addTokensResult.MapFailure<AccessAndRefreshTokenDTO>();

        // return new tokens
        return Result<AccessAndRefreshTokenDTO>.Success(generateTokensResult.Value!);
    }
    
    public void Logout(Guid userId)
    {
        cookieService.RemoveTokens();
        
        logger.LogWarning("{serviceName}.{methodName} user with id: {userId} logged out",
            nameof(AccountService), nameof(Logout), userId);
    }
}