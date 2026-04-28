using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;
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
using System.Security.Principal;
using System.Text;

namespace Custom_Builds.Core.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepositry;
        private readonly UserManager<ApplicationUser> _userManager;

        public JWTService(IConfiguration configuration,
                          IRefreshTokenRepository refreshTokenRepositry,
                          UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _refreshTokenRepositry = refreshTokenRepositry;
            _userManager = userManager;
        }


        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:AccessTokenLife"]!)),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync(HttpRequest request)
        {
            // check if tokens are valid
            var checkTokensResult = await AreRefreshTokenAndAccessTokenValidAsync(request, validateExpireDate: false);

            if (!checkTokensResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(checkTokensResult.ErrorMessage ?? "Invalid Tokens");
            }

            string accessToke = CookiesUtils.GetFromCookies(request, "AccessToken")!;

            var getPrincipalResult = GetPrincipalFromAccessToken(accessToke, ValidateLifetime: false);

            if (!getPrincipalResult.IsSuccess)
            {
                return Result<AccessAndRefreshTokenDTO>.Failure(getPrincipalResult.ErrorMessage ?? "Some thing went wrong while trying to get Principal");
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

            AccessAndRefreshTokenDTO tokens = new AccessAndRefreshTokenDTO()
            {
                AccessToken = await GenerateAccessTokenAsync(user),
                RefreshToken = await GenerateRefreshTokenAsync(user)
            };

            return Result<AccessAndRefreshTokenDTO>.Success(tokens);
        }
        public async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            byte[] bytes = new byte[64];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            string refToken = Convert.ToBase64String(bytes);

            // store refresh token in the DB
            await _refreshTokenRepositry.AddAsync(new AddRefreshTokenDTO()
            {
                ExpierDate = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenLife"]!)),
                RefreshTokenString = refToken,
                UserId = user.Id,
            });

            return refToken;
        }
        public Result<ClaimsPrincipal> GetPrincipalFromAccessToken(string accessToken , bool ValidateLifetime = true)
        {
            TokenValidationParameters tokenParams = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),

                ValidateLifetime = ValidateLifetime,
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);

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
            string? accessToken = CookiesUtils.GetFromCookies(request, "AccessToken");
            string? refreshToken = CookiesUtils.GetFromCookies(request, "RefreshToken");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return Result.Failure("No access or refresh tokens where found in cookies");
            }

            var isValidAccessTokenResult = IsValidJWTSecurityToken(accessToken, validateExpireDate);

            if (!isValidAccessTokenResult.IsSuccess)
            {
                return Result.Failure(isValidAccessTokenResult.ErrorMessage ?? "Invalid Access token");
            }




            var refreshTokenOwnerResult = await _refreshTokenRepositry.GetUserFromRefreshTokenStringAsync(refreshToken);
            var refTokenResult = await _refreshTokenRepositry.GetFromRefreshTokenStringAsync(refreshToken);

            Result result = new Result();
            result.ErrorMessage = "";
            result.StatusCode = HttpStatusCode.BadRequest;

            if (!refreshTokenOwnerResult.IsSuccess)
            {
                result.ErrorMessage += "No Owner For Refresh Token Was Found | ";
                result.StatusCode = HttpStatusCode.Unauthorized;
            }
            if (!refTokenResult.IsSuccess)
            {
                result.ErrorMessage += "No Refresh Token Was Found | ";
                result.StatusCode = HttpStatusCode.Unauthorized;
            }
            if(refTokenResult.Value!.ExpierDate <= DateTime.UtcNow)
            {
                result.ErrorMessage += "Expiered Refresh Token";
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                result.IsSuccess = false;
                return result;
            }





            var getPrincipalResult = GetPrincipalFromAccessToken(accessToken , validateExpireDate);

            if (!getPrincipalResult.IsSuccess)
            {
                return Result.Failure(getPrincipalResult.ErrorMessage ?? "Something Went Wrong While trying to get Principal");
            }



            if(!Guid.TryParse(getPrincipalResult.Value!.FindFirstValue(JwtRegisteredClaimNames.Sub) , out Guid currUserId))
            {
                return Result.Failure("Bad userId");
            }
            if (refreshTokenOwnerResult.Value!.Id != currUserId)
            {
                return Result.Failure("Unauthorized Access" , HttpStatusCode.Unauthorized);
            }

            return Result.Success();
        }
    }
}
