using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface IRegisterAccountService
    {
        Task<Result> RegisterAsync(HttpResponse response ,RegisterDTO registerInfo);
    }
}
