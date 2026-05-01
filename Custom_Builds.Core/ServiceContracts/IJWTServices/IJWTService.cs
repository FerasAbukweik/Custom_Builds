using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Custom_Builds.Core.ServiceContracts.IJWTServices
{
    public interface IJWTService
    {
        Task<Result<string>> GenerateAccessTokenAsync(ApplicationUser user);
        Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync(HttpRequest request);
        Result IsValidJWTSecurityToken(string accessToken , bool validateExpireDate = true);
        Result<ClaimsPrincipal> GetPrincipal(bool validateExpireDate = true);
        Task<Result> AreRefreshTokenAndAccessTokenValidAsync(HttpRequest request , bool validateExpireDate = true);
    }
}
