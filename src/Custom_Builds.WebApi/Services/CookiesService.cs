using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Constants;
using Custom_Builds.Core.DTO.Tokens;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Options;

namespace custom_Peripherals.Services;

public class CookiesService(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    IOptions<CookieKeys> cookieKeys) : ICookieService
{
    public Result Set(string key, string value, double lifeTimeInMinutes)
    {
        // response so we can send cookies to the browser
        HttpResponse? response = httpContextAccessor.HttpContext?.Response;
        if(response == null)
            return Result.Failure("HttpResponse is null" , HttpStatusCode.InternalServerError);

        // cookie options
        CookieOptions cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(lifeTimeInMinutes),
        };

        // add cookie to the response
        response.Cookies.Append(key, value, cookieOptions);

        return Result.Success();
    }

    public Result<string> Remove(string key)
    {
        var getToDel = Get(key);
        if (!getToDel.IsSuccess) return getToDel;
        
        Set(key, "", -1);

        return getToDel;
    }
    public Result<string> Get(string key)
    {
        // request so we can get cookies from the request
        HttpRequest? request = httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return Result<string>.Failure("HttpRequest is null", HttpStatusCode.InternalServerError);
        
        // try to get the cookie value
        if (!request.Cookies.TryGetValue(key, out string? value))
            return Result<string>.Failure("cookie was not found");

        return Result<string>.Success(value);
    }
    public Result SetTokens(AccessAndRefreshTokenDTO tokens)
    {
        // add access token to response cookies
        var addAccessTokenResult = Set(cookieKeys.Value.AccessToken, tokens.AccessToken, 
            configuration.GetValue<double>("JWT:AccessTokenLife"));

        if (!addAccessTokenResult.IsSuccess) return addAccessTokenResult;
        
        // add refresh token to response cookies
        return Set(cookieKeys.Value.RefreshToken, tokens.RefreshToken, 
            configuration.GetValue<double>("JWT:RefreshTokenLife"));
    }
    public Result<AccessAndRefreshTokenDTO> GetTokens()
    {
        // Get access token
        var getAccessTokenResult = Get(cookieKeys.Value.AccessToken);
        if (!getAccessTokenResult.IsSuccess)
            return Result<AccessAndRefreshTokenDTO>.Failure("no access token was found" , HttpStatusCode.Unauthorized);
    
        // Get refresh token
        var getRefreshTokenResult = Get(cookieKeys.Value.RefreshToken);
        if (!getRefreshTokenResult.IsSuccess)
            return Result<AccessAndRefreshTokenDTO>.Failure("no refresh token was found", HttpStatusCode.Unauthorized);

        return Result<AccessAndRefreshTokenDTO>.Success(new AccessAndRefreshTokenDTO()
        {
            AccessToken = getRefreshTokenResult.Value!,
            RefreshToken = getRefreshTokenResult.Value!
        });
    }
    public Result<AccessAndRefreshTokenDTO> RemoveTokens()
    {
        var removeAccessTokenResult = Remove(cookieKeys.Value.AccessToken);
        if (!removeAccessTokenResult.IsSuccess) return removeAccessTokenResult.MapFailure<AccessAndRefreshTokenDTO>();
        
        var removeRefreshTokenResult = Remove(cookieKeys.Value.RefreshToken);
        if(!removeRefreshTokenResult.IsSuccess) return removeRefreshTokenResult.MapFailure<AccessAndRefreshTokenDTO>();

        return Result<AccessAndRefreshTokenDTO>.Success(new AccessAndRefreshTokenDTO()
        {
            AccessToken = removeAccessTokenResult.Value!,
            RefreshToken = removeRefreshTokenResult.Value!
        });
    }

    public Result<string> GetRefreshToken()
    {
        return Get(cookieKeys.Value.RefreshToken);
    }
    
    public Result<string> GetAccessToken()
    {
        return Get(cookieKeys.Value.AccessToken);
    }
}