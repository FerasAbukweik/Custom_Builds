using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface IRegisterAccountService
    {
        Task<Result> RegisterAsync(RegisterDTO registerInfo);
    }
}
