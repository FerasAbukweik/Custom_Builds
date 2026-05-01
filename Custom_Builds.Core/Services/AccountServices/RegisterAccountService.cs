using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;

using Microsoft.AspNetCore.Http;
using Custom_Builds.Core.Services.JWTServices;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.ServiceContracts.IAccountServices;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class RegisterAccountService : IRegisterAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTService _jwtService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IConfiguration _configuration;
        private readonly IAddCookieService _addCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;

        public RegisterAccountService(UserManager<ApplicationUser> userManager,
                                      JWTService jwtService,
                                      RoleManager<ApplicationRole> roleManager,
                                      SignInManager<ApplicationUser> signinManager,
                                      IConfiguration configuration,
                                      IAddCookieService addCookieService,
                                      IGenerateRefreshTokenService generateRefreshTokenService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _signinManager = signinManager;
            _configuration = configuration;
            _addCookieService = addCookieService;
            _generateRefreshTokenService = generateRefreshTokenService;
        }


        public async Task<Result> RegisterAsync(HttpResponse response ,RegisterDTO registerInfo)
        {
            
            if (await _userManager.FindByEmailAsync(registerInfo.Email) != null)
            {
                return Result.Failure("Email is already in use");
            }



            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = registerInfo.UserName,
                Email = registerInfo.Email,
                PhoneNumber = registerInfo.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(newUser, registerInfo.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(" | ", result.Errors);
                return Result.Failure(errors);
            }


            // if role doesnt exist create new one
            if (await _roleManager.FindByNameAsync(registerInfo.role.ToString()) == null)
            {
                ApplicationRole newRole = new ApplicationRole() 
                {
                    Name = registerInfo.role.ToString()
                };

                await _roleManager.CreateAsync(newRole);
            }

            // add user to his role
            await _userManager.AddToRoleAsync(newUser, registerInfo.role.ToString());



            // generate Tokens
            var accessTokenResult = await _jwtService.GenerateAccessTokenAsync(newUser);
            if (!accessTokenResult.IsSuccess)
            {
                return Result.Failure(accessTokenResult.ErrorMessage ?? "Failed to generate access token" , accessTokenResult.StatusCode);
            }

            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(newUser);
            if (!refreshTokenResult.IsSuccess)
            {
                return Result.Failure(refreshTokenResult.ErrorMessage ?? "Failed to generate refresh token", refreshTokenResult.StatusCode);
            }

            // store Tokens in http only cookies
            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            Result addAccessTokenResult = _addCookieService.Add("AccessToken", accessTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addAccessTokenResult.IsSuccess)
            {
                return Result.Failure(addAccessTokenResult.ErrorMessage ?? "Failed to add access token to cookies" , addAccessTokenResult.StatusCode);
            }

            Result addRefreshTokenResult = _addCookieService.Add("RefreshToken", refreshTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addRefreshTokenResult.IsSuccess)
            {
                return Result.Failure(addRefreshTokenResult.ErrorMessage ?? "Failed to add refresh token to cookies", addRefreshTokenResult.StatusCode);
            }


            await _signinManager.SignInAsync(newUser, isPersistent: false);

            return Result.Success();
        }
    }
}
