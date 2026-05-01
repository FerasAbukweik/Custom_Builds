using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Custom_Builds.Core.Services.CookiesServices
{
    public class DeleteCookieService : IDeleteCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Result Delete(string key)
        {
            HttpResponse response = _httpContextAccessor.HttpContext.Response;

            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(-1),
            };

            response.Cookies.Delete(key, cookieOptions);


            return Result.Success();
        }
    }
}
