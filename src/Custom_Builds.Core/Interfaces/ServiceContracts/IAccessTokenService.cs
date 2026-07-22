using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IAccessTokenService
{
    Task<Result<string>> GenerateAccessTokenAsync(ApplicationUser user);
}