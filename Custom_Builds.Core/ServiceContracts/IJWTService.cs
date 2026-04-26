using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Custom_Builds.Core.ServiceContracts
{
    public interface IJWTService
    {
        string GenerateRefreshToken(ApplicationUser user);
        Task<string> GenerateAccessToken(ApplicationUser user);
        Task<AccessAndRefreshTokenDTO> GenerateNewAccessAndRefreshTokensAsync(HttpRequest request);
        bool IsValidJWTSecurityToken(string accessToken , bool validateExpireDate = true);
        ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken , bool validateExpireDate = true);
        Task<bool> AreRefreshTokenAndAccessTokenValidAsync(HttpRequest request);
    }
}
