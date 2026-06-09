using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICurrTokenService
{
    public interface IGetCurrUserService
    {
        Result<Guid> GetUserId();
        Result<bool> IsAdmin();
        Result<List<string>> GetRoles();
    }
}
