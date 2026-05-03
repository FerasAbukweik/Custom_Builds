using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Microsoft.AspNetCore.Http;
using System.Net;

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
            // request so we can get cookies from the request
            HttpRequest? request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return Result<string>.Failure("HttpRequest is null", HttpStatusCode.InternalServerError);
            }
            
            // try to get the cookie value
            if (!request.Cookies.TryGetValue(key, out string? value))
            {
                return Result<string>.Failure("Cannt find the cookie");
            }

            return Result<string>.Success(value);
        }
    }
}
