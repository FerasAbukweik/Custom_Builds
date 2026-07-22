using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Tokens;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface ICookieService
{
    Result Set(string key, string value, double lifeTimeInMinutes);
    Result<string> Remove(string key);
    Result<string> Get(string key);
    Result SetTokens(AccessAndRefreshTokenDTO tokens);
    Result<AccessAndRefreshTokenDTO> GetTokens();
    Result<AccessAndRefreshTokenDTO> RemoveTokens();
}