using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Microsoft.AspNetCore.Http;
using System;

namespace Custom_Builds.Core.Services.CookiesServices
{
    public class GetCookieService : IGetCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Result<string> Get(string key)
        {
            HttpRequest request = _httpContextAccessor.HttpContext.Request;

            if (request.Cookies.TryGetValue(key, out string value))
            {
                return Result<string>.Success(value);
            }

            return Result<string>.Failure("Cannt find the cookie");
        }
    }
}
