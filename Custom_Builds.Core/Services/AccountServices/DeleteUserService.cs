using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
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
        public DeleteUserService(UserManager<ApplicationUser> userManager,
                                        SignInManager<ApplicationUser> signinManager,
                                        IDeleteCookieService deleteCookieService,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _deleteCookieService = deleteCookieService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result> DeleteUserAsync(Guid userId)
        {
            // resposne so we can remove cookies from the browser
            HttpResponse? response = _httpContextAccessor.HttpContext?.Response;
            if(response == null)
            {
                return Result.Failure("Http Response is null", HttpStatusCode.InternalServerError);
            }

            // get user object so we can delete it using _userManager.DeleteAsync(user)
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result.Failure("User wasnt found" , HttpStatusCode.NotFound);
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
