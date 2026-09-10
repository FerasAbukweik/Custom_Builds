using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Account;
using Custom_Builds.Core.DTO.Auth;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IAccountService
{
    Task<Result<ApplicationUser>> DeleteUserAsync(Guid userId);
    Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerInfo, CancellationToken cancellationToken = default);
}