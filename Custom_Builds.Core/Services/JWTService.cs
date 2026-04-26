using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.ServiceContracts;
using Custom_Builds.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IRefreshTokenRepositry _refreshTokenRepositry;
        private readonly UserManager<ApplicationUser> _userManager;

        public JWTService(IConfiguration configuration,
                          IRefreshTokenRepositry refreshTokenRepositry,
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

        public async Task<AccessAndRefreshTokenDTO> GenerateNewAccessAndRefreshTokensAsync(HttpRequest request)
        {
            // check if tokens are valid
            if (!(await AreRefreshTokenAndAccessTokenValidAsync(request , validateExpireDate: false)))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            string accessToke = CookiesUtils.GetFromCookies(request, "AccessToken")!;
            string? userId = GetPrincipalFromAccessToken(accessToke , validateExpireDate: false)?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if(userId == null)
            {
                throw new SecurityTokenException("Invalid Token");
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                throw new Exception("User Not Found");
            }

            AccessAndRefreshTokenDTO tokens = new AccessAndRefreshTokenDTO()
            {
                AccessToken = await GenerateAccessTokenAsync(user),
                RefreshToken = await GenerateRefreshTokenAsync(user)
            };

            return tokens;
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
            await _refreshTokenRepositry.AddRefrehTokenAsync(new AddRefreshTokenDTO()
            {
                ExpierDate = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenLife"]!)),
                RefreshTokenString = refToken,
                UserId = user.Id,
            });

            return refToken;
        }

        public ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken , bool validateExpireDate = true)
        {
            if (accessToken == null)
            {
                throw new SecurityTokenException("Invalid Token");
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

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);

            if (validToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                   StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }

        public bool IsValidJWTSecurityToken(string accessToken , bool validateExpireDate = true)
        {
            if (accessToken == null)
            {
                throw new Exception("Access Token Not Found In Cookies");
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

            jwtSecurityTokenHandler.ValidateToken(accessToken, tokenParams, out SecurityToken validToken);

            if (validToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                   StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AreRefreshTokenAndAccessTokenValidAsync(HttpRequest request , bool validateExpireDate = true)
        {
            string? accessToken = CookiesUtils.GetFromCookies(request, "AccessToken");
            string? refreshToken = CookiesUtils.GetFromCookies(request, "RefreshToken");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            if (!IsValidJWTSecurityToken(accessToken, validateExpireDate))
            {
                return false;
            }

            ApplicationUser? refreshTokenUser = await _refreshTokenRepositry.GetUserFromRefreshTokenStringAsync(refreshToken);
            RefreshToken? refToken = await _refreshTokenRepositry.GetRefreshTokenFromRefreshTokenStringAsync(refreshToken);

            if (refreshTokenUser == null || refToken == null || refToken.ExpierDate <= DateTime.UtcNow)
            {
                return false;
            }


            ClaimsPrincipal principal = GetPrincipalFromAccessToken(accessToken , validateExpireDate);
            if(!Guid.TryParse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub) , out Guid accessTokenUserId))
            {
                return false;
            }
            if (refreshTokenUser.Id != accessTokenUserId)
            {
                return false;
            }

            return true;
        }
    }
}
