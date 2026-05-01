using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.ServiceContracts.IRefreshTokenServices
{
    public interface IGenerateRefreshTokenService
    {
        Task<Result<string>> GenerateRefreshTokenAsync(ApplicationUser user);
    }
}
