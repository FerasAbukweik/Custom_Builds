using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.Services.JWTServices;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class LoginAccountService : ILoginAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly JWTService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IAddCookieService _addCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;

        public LoginAccountService(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signinManager,
                                   JWTService jwtService,
                                   IConfiguration configuration,
                                   IAddCookieService addCookieService,
                                   IGenerateRefreshTokenService generateRefreshTokenService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _addCookieService = addCookieService;
            _generateRefreshTokenService = generateRefreshTokenService;
        }

        public async Task<Result> LoginAsync(HttpResponse response, LoginDTO loginInfo)
        {   
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginInfo.Email);

            if (user == null)
            {
                return Result.Failure("Wrong Email or Passowrd" , HttpStatusCode.NotFound);
            }


            // check password
            var result = await _signinManager.PasswordSignInAsync(user, loginInfo.Password, false, false);

            if (!result.Succeeded)
            {
                return Result.Failure("Wrong Email or Passowrd", HttpStatusCode.NotFound);
            }

            // generate Tokens
            var accessTokenResult = await _jwtService.GenerateAccessTokenAsync(user);
            if (!accessTokenResult.IsSuccess)
            {
                return Result.Failure(accessTokenResult.ErrorMessage ?? "Failed to generate access token" , accessTokenResult.StatusCode);
            }

            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess)
            {
                return Result.Failure(refreshTokenResult.ErrorMessage ?? "Failed to generate refresh token", refreshTokenResult.StatusCode);
            }

            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            Result addAccessResult =  _addCookieService.Add("AccessToken", accessTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addAccessResult.IsSuccess)
            {
                return Result.Failure(addAccessResult.ErrorMessage ?? "Failed to add access Token to cookies" , addAccessResult.StatusCode);
            }

            Result addRefreshResult = _addCookieService.Add("RefreshToken", refreshTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addRefreshResult.IsSuccess)
            {
                return Result.Failure(addRefreshResult.ErrorMessage ?? "Failed to add refresh Token to cookies", addRefreshResult.StatusCode);
            }


            await _signinManager.SignInAsync(user, false);
            return Result.Success();
        }
    }
}
