using Azure.Core;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;

namespace custom_Peripherals.MiddleWare
{
    public class GenerateNewTokensMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        public GenerateNewTokensMiddleware(IConfiguration configuration,
                                      RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context , IJWTService jwtService , IAddCookieService addCookieService)
        {
            var isValidAccessTokenRes = jwtService.IsValidJWTSecurityToken(validateExpireDate: true);
            // if access token is valid continue
            if (isValidAccessTokenRes.IsSuccess)
            {
                await _next(context);
                return;
            }

            // if access token is bad try generating new tokens
            var tokens = await jwtService.GenerateNewAccessAndRefreshTokensAsync();
            if (!tokens.IsSuccess)
            {
                await _next(context);
                return;
            }


            // using refresh token lifetime for access token so next time we can check both expired access token and refresh token
            var refreshTokenLife = _configuration.GetValue<double>("JWT:RefreshTokenLife");
            addCookieService.Add("AccessToken", tokens.Value!.AccessToken, refreshTokenLife);
            addCookieService.Add("RefreshToken", tokens.Value!.RefreshToken, refreshTokenLife);

            // add token to the request to pass authentication on current request
            context.Request.Headers["Authorization"] = $"Bearer {tokens.Value!.AccessToken}";

            await _next(context);
        }
    }


    public static class ExtensionMethodForAutoGenerateTokens
    {
        public static IApplicationBuilder UseAutoRegenerateTokens(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GenerateNewTokensMiddleware>();
        }
    }
}
