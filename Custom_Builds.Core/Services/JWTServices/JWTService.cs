using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGetCookieService _getCookieService;
        private readonly IGenerateRefreshTokenService _generateRefreshTokenService;
        private readonly IGetRefreshTokenService _getRefreshTokenService;

        public JWTService(IConfiguration configuration,
                          IGenerateRefreshTokenService generateRefreshTokenService,
                          UserManager<ApplicationUser> userManager,
                          IGetCookieService getCookieService,
                          IGetRefreshTokenService getRefreshTokenService)
        {
            _configuration = configuration;
            _generateRefreshTokenService = generateRefreshTokenService;
            _userManager = userManager;
            _getCookieService = getCookieService;
            _getRefreshTokenService = getRefreshTokenService;
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
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:AccessTokenLife"]!)),
                    signingCredentials: creds
                );

            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
        public async Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync()
        {
            // Get access and refresh tokens

            // Get access token
            var getAccessTokenResult = _getCookieService.Get("AccessToken");
            if (!getAccessTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getAccessTokenResult.ErrorMessage ?? "Failed to get access token", getAccessTokenResult.StatusCode);
            }

            // Get refresh token
            var getRefreshTokenResult = _getCookieService.Get("RefreshToken");
            if (!getRefreshTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getRefreshTokenResult.ErrorMessage ?? "Failed to get refresh token", getRefreshTokenResult.StatusCode);
            }


            // check if tokens are valid
            var checkTokensResult = await AreRefreshTokenAndAccessTokenValidAsync(getAccessTokenResult.Value!, getRefreshTokenResult.Value!, validateAccessTokenExpireDate: false);
            if (!checkTokensResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(checkTokensResult.ErrorMessage ?? "Invalid Tokens", checkTokensResult.StatusCode);
            }

            // get access token principal
            var getPrincipalResult = GetPrincipal(getAccessTokenResult.Value! , validateExpireDate: false);
            if (!getPrincipalResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getPrincipalResult.ErrorMessage ?? "Some thing went wrong while trying to get Principal", getPrincipalResult.StatusCode);
            }

            // get user id from principal
            string? userId = getPrincipalResult.Value!.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if(userId == null)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure("Bad currUser Id");
            }

            // get user
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure("User Not Found");
            }


            // generate new tokens

            // generate access token
            var accessTokenResult = await GenerateAccessTokenAsync(user);
            if (!accessTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(accessTokenResult.ErrorMessage ?? "Failed to generate access token", accessTokenResult.StatusCode);
            }

            // generate refresh token
            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(refreshTokenResult.ErrorMessage ?? "Failed to generate refresh token", refreshTokenResult.StatusCode);
            }


            // return result
            AccessAndRefreshTokenDTO tokens = new AccessAndRefreshTokenDTO()
            {
                AccessToken = accessTokenResult.Value!,
                RefreshToken = refreshTokenResult.Value!.RefreshTokenString
            };

            return Result<AccessAndRefreshTokenDTO>.Success(tokens);
        }
        public Result<ClaimsPrincipal> GetPrincipal(string accessToken , bool validateExpireDate = true)
        {
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

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            // get access token principal
            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);


            // check if token is valid
            if (validToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                   StringComparison.InvariantCultureIgnoreCase))
            {
                return Result<ClaimsPrincipal>.Failure("Invalid Access Token");
            }

            return Result<ClaimsPrincipal>.Success(principal);
        }
        public Result IsValidJWTSecurityToken(string accessToken , bool validateExpireDate = true)
        {
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

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);

            if (validToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                   StringComparison.InvariantCultureIgnoreCase))
            {
                return Result.Failure("Bad Access Token");
            }

            return Result.Success();
        }
        public async Task<Result> AreRefreshTokenAndAccessTokenValidAsync(string accessToken , string refreshToken , bool validateAccessTokenExpireDate = true)
        {
            // check access token if its valid
            var isValidAccessTokenResult = IsValidJWTSecurityToken(accessToken, validateAccessTokenExpireDate);
            if (!isValidAccessTokenResult.IsSuccess) return isValidAccessTokenResult;


            // get refresh token object so we can access its expire date and user id
            var refTokenResult = await _getRefreshTokenService.GetFromRefreshTokenString(refreshToken);

            // this to collect possible errors
            Result result = new Result();
            result.ErrorMessage = "";
            result.StatusCode = HttpStatusCode.BadRequest;
            result.IsSuccess = true;


            // collect possible errors
            if (!refTokenResult.IsSuccess)
            {
                result.ErrorMessage += "No Refresh Token Was Found | ";
                result.StatusCode = HttpStatusCode.Unauthorized;
                result.IsSuccess = false;
            }
            if (refTokenResult.IsSuccess && refTokenResult.Value!.ExpierDate <= DateTime.UtcNow)
            {
                result.ErrorMessage += "Expiered Refresh Token";
                result.IsSuccess = false;
            }
            if (!result.IsSuccess)
            {
                return result;
            }


            // get access token principal so we can access its userId
            var getPrincipalResult = GetPrincipal(accessToken , validateExpireDate : false);
            if (!getPrincipalResult.IsSuccess) return getPrincipalResult;

            // get user id from principal
            if (!Guid.TryParse(getPrincipalResult.Value!.FindFirstValue(JwtRegisteredClaimNames.Sub) , out Guid claimsUserId))
            {
                return Result.Failure("Invalid Token");
            }


            // check if access token and refresh token belong to the same user
            // User is virtual so we can access it via lazy loading without .including
            if (refTokenResult.Value!.User?.Id != claimsUserId)
            {
                return Result.Failure("Invalid Token", HttpStatusCode.Unauthorized);
            }

            return Result.Success();
        }
    }
}
