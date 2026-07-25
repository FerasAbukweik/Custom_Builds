
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Tokens;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface ITokensService
{ 
        Task<Result<AccessAndRefreshTokenDTO>> GenerateTokens(ApplicationUser user,
        CancellationToken cancellationToken = default);
}