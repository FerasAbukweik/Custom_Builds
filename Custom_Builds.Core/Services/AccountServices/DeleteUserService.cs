using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.Services.CurrUserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class DeleteUserService : IDeleteUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IDeleteCookieService _deleteCookieService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetCurrUserService _getCurrUserService;
        public DeleteUserService(UserManager<ApplicationUser> userManager,
                                        SignInManager<ApplicationUser> signinManager,
                                        IDeleteCookieService deleteCookieService,
                                        IHttpContextAccessor httpContextAccessor,
                                        IGetCurrUserService getCurrUserService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _deleteCookieService = deleteCookieService;
            _httpContextAccessor = httpContextAccessor;
            _getCurrUserService = getCurrUserService;
        }
        public async Task<Result> DeleteUserAsync(Guid? id)
        {
            // get target user id
            var getTargetUserIdRes = _getCurrUserService.GetTargetUserId(id);
            if (!getTargetUserIdRes.IsSuccess) return getTargetUserIdRes;

            // get user object so we can delete it using _userManager.DeleteAsync(user) and check if its an admin
            ApplicationUser? user = await _userManager.FindByIdAsync(getTargetUserIdRes.Value!.ToString());
            if (user == null)
            {
                return Result.Failure("User wasnt found", HttpStatusCode.NotFound);
            }

            // if target user is admin stop
            if(await _userManager.IsInRoleAsync(user , RoleEnums.Admin.ToString()))
            {
                return Result.Failure("Forbidden to delete an admin" , HttpStatusCode.Forbidden);
            }
            
            // remove User from IdentityUser table
            // also removes all user refreshTokens because DeleteBehavior.cascade
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                string errors = string.Join(" | ", result.Errors);
                return Result.Failure(errors);
            }

            //remove Token Cookies from browser
            await _signinManager.SignOutAsync();

            // delete access token from cookies
            Result delAccessResult = _deleteCookieService.Delete("AccessToken");
            if (!delAccessResult.IsSuccess) return delAccessResult;

            // delete refresh token from cookies
            Result delrefResult = _deleteCookieService.Delete("RefreshToken");
            if (!delrefResult.IsSuccess) return delrefResult;

            return Result.Success();
        }
    }
}
