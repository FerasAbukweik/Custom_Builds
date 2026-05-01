using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;

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
            await _signinManager.SignOutAsync();

            Result deleteAccessResult =  _deleteCookieService.Delete("AccessToken");
            if (!deleteAccessResult.IsSuccess)
            {
                return Result.Failure(deleteAccessResult.ErrorMessage ?? "Failed to delete access token from cookies", deleteAccessResult.StatusCode);
            }

            var getRefreshResult = _getCookieService.Get("RefreshToken");

            Result deleteRefreshResult = _deleteCookieService.Delete("RefreshToken");
            if (!deleteRefreshResult.IsSuccess)
            {
                return Result.Failure(deleteRefreshResult.ErrorMessage ?? "Failed to delete access token from cookies" , deleteRefreshResult.StatusCode);
            }


            if (getRefreshResult.IsSuccess)
            {
                var revokeResult = await _removeRefreshTokenService.RemoveByRefreshTokenStringAsync(getRefreshResult.Value!);
                if (!revokeResult.IsSuccess)
                {
                    return Result.Failure(revokeResult.ErrorMessage ?? "Failed to revoke refresh token", revokeResult.StatusCode);
                }
            }

            return Result.Success();
        }
    }
}
