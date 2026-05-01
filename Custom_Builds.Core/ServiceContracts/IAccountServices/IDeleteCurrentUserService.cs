using Custom_Builds.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Custom_Builds.Core.ServiceContracts.IAccountServices
{
    public interface IDeleteCurrentUserService
    {
        Task<Result> DeleteUserAsync(HttpResponse response, ClaimsPrincipal principal);
    }
}
