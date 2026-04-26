using Microsoft.AspNetCore.Http;
using System;

namespace Custom_Builds.Core.Utils
{
    public static class CookiesUtils
    {
        public static void AddToCookies(HttpResponse response ,string key ,string value , double lifeTime)
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(lifeTime),
            };

            response.Cookies.Append(key, value , cookieOptions);
        }

        public static void DeleteCookie(HttpResponse response, string key)
        {
            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(-1),
                Path = "/"
            };

            response.Cookies.Delete(key, cookieOptions);
        }

        public static string? GetFromCookies(HttpRequest request , string key)
        {
            if(request.Cookies.TryGetValue(key , out string value))
            {
                return value;
            }

            return null;
        }
    }
}
