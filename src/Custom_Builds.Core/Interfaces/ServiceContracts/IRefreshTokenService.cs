using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Tokens;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IRefreshTokenService
{
    Task<Result<RefreshTokenDTO>> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
}