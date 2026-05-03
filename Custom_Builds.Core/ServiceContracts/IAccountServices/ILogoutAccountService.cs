using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface ILogoutAccountService
    {
        Task<Result> LogoutAsync();
    }
}
