using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICookieServices;
using Microsoft.AspNetCore.Http;
using System.Net;

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
            // response so we can send response to delete the cookie
            HttpResponse? response = _httpContextAccessor.HttpContext?.Response;
            if (response == null)
            {
                return Result.Failure("HttpResponse is null", HttpStatusCode.InternalServerError);
            }

            // cookie options
            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                // old date so browser will delete the cookie
                Expires = DateTime.UtcNow.AddMinutes(-1),
            };

            // add the outdated cookie to the response
            response.Cookies.Delete(key, cookieOptions);


            return Result.Success();
        }
    }
}
