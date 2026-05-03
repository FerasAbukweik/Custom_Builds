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
using System.Net;

namespace Custom_Builds.Core.Services.AccountServices
{
    public class RegisterAccountService : IRegisterAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJWTService _jwtService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IConfiguration _configuration;
        private readonly IAddCookieService _addCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterAccountService(UserManager<ApplicationUser> userManager,
                                      IJWTService jwtService,
                                      RoleManager<ApplicationRole> roleManager,
                                      SignInManager<ApplicationUser> signinManager,
                                      IConfiguration configuration,
                                      IAddCookieService addCookieService,
                                      IGenerateRefreshTokenService generateRefreshTokenService,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _signinManager = signinManager;
            _configuration = configuration;
            _addCookieService = addCookieService;
            _generateRefreshTokenService = generateRefreshTokenService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Result> RegisterAsync(RegisterDTO registerInfo)
        {
            // resposne so we can send new tokens to cookies
            HttpResponse? response = _httpContextAccessor.HttpContext?.Response;
            if (response == null)
            {
                return Result.Failure("Http Response is null", HttpStatusCode.InternalServerError);
            }

            // check if email already exists
            if (await _userManager.FindByEmailAsync(registerInfo.Email) != null)
            {
                return Result.Failure("Email is already in use");
            }


            // new user to add
            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = registerInfo.UserName,
                Email = registerInfo.Email,
                PhoneNumber = registerInfo.PhoneNumber,
            };

            // add user to identityUser table
            var addUserResult = await _userManager.CreateAsync(newUser, registerInfo.Password);
            if (!addUserResult.Succeeded)
            {
                string errors = string.Join(" | ", addUserResult.Errors);
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

            // generate access token
            var accessTokenResult = await _jwtService.GenerateAccessTokenAsync(newUser);
            if (!accessTokenResult.IsSuccess) return accessTokenResult;

            // generate refresh token
            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(newUser);
            if (!refreshTokenResult.IsSuccess) return refreshTokenResult;


            // store Tokens in http only cookies

            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            // add access token to cookies
            Result addAccessTokenResult = _addCookieService.Add("AccessToken", accessTokenResult.Value!, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addAccessTokenResult.IsSuccess) return addAccessTokenResult;

            // add refresh token to cookies
            Result addRefreshTokenResult = _addCookieService.Add("RefreshToken", refreshTokenResult.Value!.RefreshTokenString, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            if (!addRefreshTokenResult.IsSuccess) return addRefreshTokenResult;

            // add identity tokens to cookies
            await _signinManager.SignInAsync(newUser, isPersistent: true);

            return Result.Success();
        }
    }
}
