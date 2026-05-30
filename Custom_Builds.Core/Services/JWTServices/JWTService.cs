using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.Services.CurrTokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Custom_Builds.Core.Services.JWTServices
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly IGetCookieService _getCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;
        private readonly IGetRefreshTokenService _getRefreshTokenService;
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public JWTService(IConfiguration configuration,
                          IGenerateRefreshTokenService generateRefreshTokenService,
                          UserManager<ApplicationUser> userManager,
                          IGetCookieService getCookieService,
                          IGetRefreshTokenService getRefreshTokenService,
                          IGetCurrUserService getCurrUserService)
        {
            _configuration = configuration;
            _generateRefreshTokenService = generateRefreshTokenService;
            _getCookieService = getCookieService;
            _getRefreshTokenService = getRefreshTokenService;
            _getCurrUserService = getCurrUserService;
            _userManager = userManager;
        }


        public async Task<Result<string>> GenerateAccessTokenAsync(ApplicationUser user)
        {
            // claims
            List<Claim> claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
    
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            // addRoles
            var roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("JWT:AccessTokenLife")),
                    signingCredentials: creds
                );

            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
        public async Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync()
        {
            // Get access and refresh tokens

            // Get access token
            var getAccessTokenResult = _getCookieService.Get("AccessToken");
            if (!getAccessTokenResult.IsSuccess) return Result<AccessAndRefreshTokenDTO>.Failure("no access token was found" , HttpStatusCode.Unauthorized);

            // Get refresh token
            var getRefreshTokenResult = _getCookieService.Get("RefreshToken");
            if (!getRefreshTokenResult.IsSuccess) return Result<AccessAndRefreshTokenDTO>.Failure("no refresh token was found", HttpStatusCode.Unauthorized);


            // check if tokens are valid
            var checkTokensResult = await AreRefreshTokenAndAccessTokenValidAsync(getAccessTokenResult.Value!, getRefreshTokenResult.Value!, validateAccessTokenExpireDate: false);
            if (!checkTokensResult.IsSuccess) return checkTokensResult.MapFailure<AccessAndRefreshTokenDTO>();

            var getCurrUserIdResult = _getCurrUserService.GetUserId();
            if (!getCurrUserIdResult.IsSuccess) return getCurrUserIdResult.MapFailure<AccessAndRefreshTokenDTO>();

            // get user
            ApplicationUser? user = await _userManager.FindByIdAsync(getCurrUserIdResult.Value!.ToString());
            if(user == null)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure("User Not Found");
            }

            // generate new tokens

            // generate access token
            var accessTokenResult = await GenerateAccessTokenAsync(user);
            if (!accessTokenResult.IsSuccess) return accessTokenResult.MapFailure<AccessAndRefreshTokenDTO>();

            // generate refresh token
            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess) return refreshTokenResult.MapFailure<AccessAndRefreshTokenDTO>();


            // return result
            AccessAndRefreshTokenDTO tokens = new AccessAndRefreshTokenDTO()
            {
                AccessToken = accessTokenResult.Value!,
                RefreshToken = refreshTokenResult.Value!.RefreshTokenString
            };

            return Result<AccessAndRefreshTokenDTO>.Success(tokens);
        }
        public Result IsValidJWTSecurityToken(string? accessToken = null , bool validateExpireDate = true)
        {
            if(accessToken == null)
            {
                var getAccessTokesRes = _getCookieService.Get("AccessToken");
                if (!getAccessTokesRes.IsSuccess) return Result.Failure("No access token" , HttpStatusCode.Unauthorized);

                accessToken = getAccessTokesRes.Value!;
            }

            TokenValidationParameters tokenParams = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),

                ValidateLifetime = validateExpireDate,
            };


            try
            {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);

                if (validToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                       StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result.Failure("Bad Access Token" , HttpStatusCode.Unauthorized);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("Bad Access Token" , HttpStatusCode.Unauthorized);
            }
            
        }
        public async Task<Result> AreRefreshTokenAndAccessTokenValidAsync(string? accessToken = null , string? refreshToken = null , bool validateAccessTokenExpireDate = true)
        {
            if (accessToken == null)
            {
                var getAccessTokesRes = _getCookieService.Get("AccessToken");
                if (!getAccessTokesRes.IsSuccess) return Result.Failure("No access token" , HttpStatusCode.Unauthorized);

                accessToken = getAccessTokesRes.Value!;
            }

            if (refreshToken == null)
            {
                var getRefreshTokenRes = _getCookieService.Get("RefreshToken");
                if (!getRefreshTokenRes.IsSuccess) return Result.Failure("No refresh token" , HttpStatusCode.Unauthorized);

                refreshToken = getRefreshTokenRes.Value!;
            }

            // check access token if its valid
            var isValidAccessTokenResult = IsValidJWTSecurityToken(accessToken, validateAccessTokenExpireDate);
            if (!isValidAccessTokenResult.IsSuccess) return isValidAccessTokenResult;


            // get refresh token object so we can access its expire date and user id
            var refTokenResult = await _getRefreshTokenService.GetFromRefreshTokenString(refreshToken);
            if (!refTokenResult.IsSuccess) return refTokenResult;

            if (refTokenResult.Value!.ExpierDate <= DateTime.UtcNow)
            {
                return Result.Failure("Expiered Refresh Token" , HttpStatusCode.Unauthorized);
            }    



            var getUserIdResult = _getCurrUserService.GetUserId();
            if(!getUserIdResult.IsSuccess) return getUserIdResult;


            // check if access token and refresh token belong to the same user
            if (refTokenResult.Value!.UserId != getUserIdResult.Value!)
            {
                return Result.Failure("Invalid Token", HttpStatusCode.Unauthorized);
            }

            return Result.Success();
        }
    }
}
