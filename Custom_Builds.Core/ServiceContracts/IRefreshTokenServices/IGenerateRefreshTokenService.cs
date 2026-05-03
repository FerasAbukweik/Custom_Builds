using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IRefreshTokenServices
{
    public interface IGenerateRefreshTokenService
    {
        Task<Result<RefreshTokenDTO>> GenerateRefreshTokenAsync(ApplicationUser user);
    }
}
