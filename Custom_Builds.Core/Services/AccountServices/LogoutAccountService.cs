using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class LogoutAccountService : ILogoutAccountService
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDeleteCookieService _deleteCookieService;
        private readonly IGetCookieService _getCookieService;
        private readonly IRemoveRefreshTokenService _removeRefreshTokenService;

        public LogoutAccountService(SignInManager<ApplicationUser> signinManager,
                                    IHttpContextAccessor httpContextAccessor,
                                    IDeleteCookieService deleteCookieService,
                                    IGetCookieService getCookieService,
                                    IRemoveRefreshTokenService removeRefreshTokenService)
        {
            _signinManager = signinManager;
            _httpContextAccessor = httpContextAccessor;
            _deleteCookieService = deleteCookieService;
            _getCookieService = getCookieService;
            _removeRefreshTokenService = removeRefreshTokenService;
        }


        public async Task<Result> LogoutAsync()
        {
            // if user have refresh token in cookies, remove it from database
            var getRefreshTokenResult = _getCookieService.Get("RefreshToken");
            if (getRefreshTokenResult.IsSuccess)
            {
                var removeRefreshTokenResult = await _removeRefreshTokenService.RemoveByRefreshTokenStringAsync(getRefreshTokenResult.Value!);
                if (!removeRefreshTokenResult.IsSuccess) return removeRefreshTokenResult;
            }   


            // remove identity tokens from cookies
            await _signinManager.SignOutAsync();

            // remove access token from cookies
            Result deleteAccessResult = _deleteCookieService.Delete("AccessToken");
            if (!deleteAccessResult.IsSuccess) return deleteAccessResult;

            // remove refresh token from cookies
            Result deleteRefreshResult = _deleteCookieService.Delete("RefreshToken");
            if (!deleteRefreshResult.IsSuccess) return deleteRefreshResult;

            return Result.Success();
        }
    }
}
