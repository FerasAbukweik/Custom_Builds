using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.ICurrTokenService
{
    public interface ICurrTokenService
    {
        Result<Guid> GetUserId();
        Result<Guid> GetTargetUserId(Guid? suppliedId);
        Result<bool> IsAdmin();
        Result<List<string>> GetRoles();
    }
}
