using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.ServiceContracts;
using Custom_Builds.Core.Utils;
using Custom_Builds.Infrastructure.DBcontext;
using Custom_Builds.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IJWTService _jwtService;
        private readonly IRefreshTokenRepositry _refreshTokenRepositry;
        private readonly IConfiguration _configuration;

        public AccountController(
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager,
                SignInManager<ApplicationUser> signinManager,
                IJWTService jwtService,
                IRefreshTokenRepositry refreshTokenRepositry,
                IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signinManager = signinManager;
            _jwtService = jwtService;
            _refreshTokenRepositry = refreshTokenRepositry;
            _configuration = configuration;
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO registerInfo)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();
                return BadRequest(errors);
            }


            if (await _userManager.Users.AnyAsync(u => u.Email == registerInfo.Email))
            {
                return BadRequest("Email Already Used");
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
                return BadRequest(errors);
            }


            // if role doesnt exist create new one
            if(!(await _roleManager.Roles.AnyAsync(r => r.Name == registerInfo.role.ToString())))
            {
                ApplicationRole newRole = new ApplicationRole() { Name =  registerInfo.role.ToString() };
                await _roleManager.CreateAsync(newRole);
            }

            // add user to his role
            await _userManager.AddToRoleAsync(newUser, registerInfo.role.ToString());



            // generate Tokens
            string accessToken = await _jwtService.GenerateAccessTokenAsync(newUser);
            string refreshToken = await _jwtService.GenerateRefreshTokenAsync(newUser);

            // store Tokens in http only cookies
            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            CookiesUtils.AddToCookies(Response, "AccessToken", accessToken, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            CookiesUtils.AddToCookies(Response, "RefreshToken", refreshToken, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));

            await _signinManager.SignInAsync(newUser , isPersistent: false);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO loginInfo)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginInfo.Email);

            if(user == null)
            {
                return Problem("Wrong Email or Passowrd");
            }


            // check password
            var result = await _signinManager.PasswordSignInAsync(user, loginInfo.Password , false , false);

            if (!result.Succeeded)
            {
                return Problem("Wrong Email or Passowrd");
            }

            // generate Tokens
            string accessToken = await _jwtService.GenerateAccessToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken(user);

            // use RefreshToken lifetime for AccessToken
            // so we can require both expiered Date accessToken and valid refresh token for more security
            CookiesUtils.AddToCookies(Response, "AccessToken", accessToken, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));
            CookiesUtils.AddToCookies(Response, "RefreshToken", refreshToken, Double.Parse(_configuration["JWT:RefreshTokenLife"]!));

            await _signinManager.SignInAsync(user , false);
            return Ok();

        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            string? userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if(userId == null)
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            // remove User from IdentityUser table
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join(" | ", result.Errors);
                return BadRequest(errors);
            }

            //remove Token Cookies from browser
            await _signinManager.SignOutAsync();
            CookiesUtils.DeleteCookie(Response, "AccessToken");
            CookiesUtils.DeleteCookie(Response, "RefreshToken");

            // remove All current User refreshTokens
            await _refreshTokenRepositry.RemoveUserRefreshTokensAsync(Guid.Parse(userId));

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            CookiesUtils.DeleteCookie(Response, "AccessToken");

            string? refreshToken = CookiesUtils.GetFromCookies(Request, "RefreshToken");
            CookiesUtils.DeleteCookie(Response, "RefreshToken");

            if(refreshToken != null)
            {
                await _refreshTokenRepositry.RemoveRefreshTokenByRefreshTokenStringAsync(refreshToken);
            }

            return Ok();
        }
    }
}
