using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface ILoginAccountService
    {
        Task<Result> LoginAsync(HttpResponse response , LoginDTO loginInfo);
    }
}
