using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface ILogoutAccountService
    {
        Task<Result> LogoutAsync();
    }
}
