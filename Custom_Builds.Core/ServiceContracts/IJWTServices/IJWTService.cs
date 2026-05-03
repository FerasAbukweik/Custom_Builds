using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System.Security.Claims;

namespace Custom_Builds.Core.ServiceContracts.IJWTServices
{
    public interface IJWTService
    {
        Task<Result<string>> GenerateAccessTokenAsync(ApplicationUser user);
        Task<Result<AccessAndRefreshTokenDTO>> GenerateNewAccessAndRefreshTokensAsync();
        Result IsValidJWTSecurityToken(string accessToken , bool validateExpireDate = true);
        Result<ClaimsPrincipal> GetPrincipal(string accessToken , bool validateExpireDate = true);
        Task<Result> AreRefreshTokenAndAccessTokenValidAsync(string accessToken , string refreshToken , bool validateExpireDate = true);
    }
}
