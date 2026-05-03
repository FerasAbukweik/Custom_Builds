using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class LoginAccountService : ILoginAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IJWTService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IAddCookieService _addCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginAccountService(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signinManager,
                                   IJWTService jwtService,
                                   IConfiguration configuration,
                                   IAddCookieService addCookieService,
                                   IGenerateRefreshTokenService generateRefreshTokenService,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _addCookieService = addCookieService;
            _generateRefreshTokenService = generateRefreshTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> LoginAsync(LoginDTO loginInfo)
        {
            // response so we can write new tokens to user browser cookies
            HttpResponse? response = _httpContextAccessor.HttpContext?.Response;
            if (response == null)
            {
                return Result.Failure("Http Response is null", HttpStatusCode.InternalServerError);
            }

            // find user by email so we can check email exist and so we can verify password using PasswordSignInAsync(user, loginInfo.Password, false, false)
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

            // generate accessToken
            var accessTokenResult = await _jwtService.GenerateAccessTokenAsync(user);
            if (!accessTokenResult.IsSuccess) return accessTokenResult;

            // generate refreshToken
            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess) return refreshTokenResult;


            // add tokens to Cookies

            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            Result addAccessResult =  _addCookieService.Add("AccessToken", accessTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addAccessResult.IsSuccess) return addAccessResult;

            Result addRefreshResult = _addCookieService.Add("RefreshToken", refreshTokenResult.Value!.RefreshTokenString, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addRefreshResult.IsSuccess) return addRefreshResult;

            // remove identity tokens from browser cookies
            await _signinManager.SignInAsync(user, false);

            return Result.Success();
        }
    }
}