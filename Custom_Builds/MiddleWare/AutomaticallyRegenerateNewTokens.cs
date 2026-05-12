using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;

namespace custom_Peripherals.MiddleWare
{
    public class RefreshTokenMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(IConfiguration configuration,
                                      RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context , IJWTService _jwtService , IAddCookieService _addCookieService)
        {
            await _next(context);


            // run only if unauthorized and first time trying to refresh the tokens
            if (context.Response.StatusCode != 401 || context.Items.ContainsKey("TokenRefreshed")) return;

            // getRefreshTokens refresh
            var tokens = await _jwtService.GenerateNewAccessAndRefreshTokensAsync();
            if (!tokens.IsSuccess) return;


            // using refresh token lifetime for access token so next time we can check both expired access token and refresh token
            var refreshTokenLife = _configuration.GetValue<double>("JWT:RefreshTokenLife");
            _addCookieService.Add("AccessToken", tokens.Value!.AccessToken, refreshTokenLife);
            _addCookieService.Add("RefreshToken", tokens.Value!.RefreshToken, refreshTokenLife);


            // reset response and retry
            context.Response.Clear();
            context.Response.StatusCode = 200;

            // mark refreshed tokens
            context.Items["TokenRefreshed"] = true;

            await _next(context);
        }
    }


    public static class ExtensionMethodForAutoGenerateTokens
    {
        public static IApplicationBuilder UseAutoRefreshTokens(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RefreshTokenMiddleware>();
        }
    }
}
