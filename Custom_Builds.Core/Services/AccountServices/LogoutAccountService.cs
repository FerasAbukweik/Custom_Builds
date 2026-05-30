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
        private readonly IDeleteCookieService _deleteCookieService;

        public LogoutAccountService(SignInManager<ApplicationUser> signinManager,
                                    IDeleteCookieService deleteCookieService)
        {
            _signinManager = signinManager;
            _deleteCookieService = deleteCookieService;
        }


        public async Task<Result> LogoutAsync()
        {
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
