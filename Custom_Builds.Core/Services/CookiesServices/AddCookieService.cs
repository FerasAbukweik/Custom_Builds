using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CookieServices;
using Microsoft.AspNetCore.Http;
using System;

namespace Custom_Builds.Core.Services.CookiesServices
{
    public class AddCookieService : IAddCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Result Add(string key, string value, double lifeTime)
        {
            HttpResponse response = _httpContextAccessor.HttpContext.Response;

            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(lifeTime),
            };

            response.Cookies.Append(key, value, cookieOptions);

            return Result.Success();
        }
    }
}
