using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Custom_Builds.Core.ServiceContracts.IJWTServices;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync(HttpRequest request)
        {
            // check if tokens are valid
            var checkTokensResult = await AreRefreshTokenAndAccessTokenValidAsync(request, validateExpireDate: false);

            if (!checkTokensResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(checkTokensResult.ErrorMessage ?? "Invalid Tokens", checkTokensResult.StatusCode);
            }

            
            var getPrincipalResult = GetPrincipal(validateExpireDate: false);
            if (!getPrincipalResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getPrincipalResult.ErrorMessage ?? "Some thing went wrong while trying to get Principal", getPrincipalResult.StatusCode);
            }

            string? userId = getPrincipalResult.Value!.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if(userId == null)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure("Bad currUser Id");
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure("User Not Found");
            }

            var accessTokenResult = await GenerateAccessTokenAsync(user);
            if (!accessTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(accessTokenResult.ErrorMessage ?? "Failed to generate access token", accessTokenResult.StatusCode);
            }

            var refreshTokenResult = await _generateRefreshTokenService.GenerateRefreshTokenAsync(user);
            if (!refreshTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(refreshTokenResult.ErrorMessage ?? "Failed to generate refresh token", refreshTokenResult.StatusCode);
            }

            AccessAndRefreshTokenDTO tokens = new AccessAndRefreshTokenDTO()
            {
                AccessToken = accessTokenResult.Value!,
                RefreshToken = refreshTokenResult.Value!
            };

            return Result<AccessAndRefreshTokenDTO>.Success(tokens);
        }
        public Result<ClaimsPrincipal> GetPrincipal(bool validateExpireDate = true)
        {
            var getaccessTokenResult = _getCookieService.Get("AccessToken");

            if (!getaccessTokenResult.IsSuccess)
            {
                return Result<ClaimsPrincipal>.Failure(getaccessTokenResult.ErrorMessage ?? "Failed to get access token", getaccessTokenResult.StatusCode);
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

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(getaccessTokenResult.Value!, tokenParams, out SecurityToken validToken);

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
        public async Task<Result> AreRefreshTokenAndAccessTokenValidAsync(HttpRequest request , bool validateExpireDate = true)
        {
            var getaccessTokenResult = _getCookieService.Get("AccessToken");

            if (!getaccessTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getaccessTokenResult.ErrorMessage ?? "Failed to get access token", getaccessTokenResult.StatusCode);
            }

            var getRefreshTokenResult = _getCookieService.Get("RefreshToken");

            if (!getRefreshTokenResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getRefreshTokenResult.ErrorMessage ?? "Failed to get access token", getRefreshTokenResult.StatusCode);
            }



            var isValidAccessTokenResult = IsValidJWTSecurityToken(getaccessTokenResult.Value!, validateExpireDate);

            if (!isValidAccessTokenResult.IsSuccess)
            {
                return Result.Failure(isValidAccessTokenResult.ErrorMessage ?? "Invalid Access token", isValidAccessTokenResult.StatusCode);
            }




            var refTokenResult = await _getRefreshTokenService.GetFromRefreshTokenString(getRefreshTokenResult.Value!);

            Result result = new Result();
            result.ErrorMessage = "";
            result.StatusCode = HttpStatusCode.BadRequest;

            if (!refTokenResult.IsSuccess)
            {
                result.ErrorMessage += "No Refresh Token Was Found | ";
                result.StatusCode = HttpStatusCode.Unauthorized;
            }
            if (refTokenResult.IsSuccess && refTokenResult.Value!.ExpierDate <= DateTime.UtcNow)
            {
                result.ErrorMessage += "Expiered Refresh Token";
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                result.IsSuccess = false;
                return result;
            }




            var getPrincipalResult = GetPrincipal(validateExpireDate : false);

            if (!getPrincipalResult.IsSuccess)
            {
                return Result.Failure(getPrincipalResult.ErrorMessage ?? "Something Went Wrong While trying to get Principal", getPrincipalResult.StatusCode);
            }



            if(!Guid.TryParse(getPrincipalResult.Value!.FindFirstValue(JwtRegisteredClaimNames.Sub) , out Guid claimsUserId))
            {
                return Result.Failure("Invalid Token");
            }
            if (refTokenResult.Value!.User?.Id != claimsUserId)
            {
                return Result.Failure("Invalid Token", HttpStatusCode.Unauthorized);
            }

            return Result.Success();
        }
    }
}
