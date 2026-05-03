using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface ILoginAccountService
    {
        Task<Result> LoginAsync(LoginDTO loginInfo);
    }
}
