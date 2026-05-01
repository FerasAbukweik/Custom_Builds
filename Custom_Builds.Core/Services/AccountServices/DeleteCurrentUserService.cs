using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Custom_Builds.Core.Services.AccountServices
{
    internal class DeleteCurrentUserService : IDeleteCurrentUserService
    {
        private readonly IJWTService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IDeleteCookieService _deleteCookieService;
        public DeleteCurrentUserService(IJWTService jwtService,
                                        UserManager<ApplicationUser> userManager,
                                        SignInManager<ApplicationUser> signinManager,
                                        IDeleteCookieService deleteCookieService)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signinManager = signinManager;
            _deleteCookieService = deleteCookieService;
        }
        public async Task<Result> DeleteUserAsync(HttpResponse response , ClaimsPrincipal principal)
        {
            string? userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId == null)
            {
                return Result.Failure("User wasnt found" , HttpStatusCode.NotFound);
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

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
                return Result.Failure(errors, HttpStatusCode.BadRequest);
            }

            //remove Token Cookies from browser
            await _signinManager.SignOutAsync();
            Result delAccessResult = _deleteCookieService.Delete("AccessToken");
            if (!delAccessResult.IsSuccess)
            {
                return Result.Failure(delAccessResult.ErrorMessage ?? "Failed To Delete Access Token From Cookies", delAccessResult.StatusCode);
            }

            Result delrefResult = _deleteCookieService.Delete("RefreshToken");
            if (!delrefResult.IsSuccess)
            {
                return Result.Failure(delrefResult.ErrorMessage ?? "Failed To Delete Access Token From Cookies", delrefResult.StatusCode);
            }

            return Result.Success();
        }
    }
}
