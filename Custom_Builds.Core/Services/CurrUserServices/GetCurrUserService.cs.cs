using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Custom_Builds.Core.Services.CurrTokenService
{
    public class GetCurrUserService : IGetCurrUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<Guid> GetUserId()
        {
            // get current user id from claim
            string? userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // if no user id claim found, return failure result
            if (userIdString == null)
            {
                return Result<Guid>.Failure("Cannot find current user Id");
            }

            // if invalid guid format, return failure result
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Result<Guid>.Failure("Invalid current user Id format");
            }

            return Result<Guid>.Success(userId);
        }
        public Result<bool> IsAdmin()
        {
            // is current user an admin ?
            var isAdmin = _httpContextAccessor.HttpContext?.User.IsInRole(RoleEnums.Admin.ToString());
            if (isAdmin == null || !isAdmin.Value)
            {
                // if cannt determine user role , return failure result
                return Result<bool>.Failure("Failed to determine user role.", HttpStatusCode.InternalServerError);
            }

            return Result<bool>.Success(true);
        }
        public Result<List<string>> GetRoles()
        {
            // get current user roles from claim
            var roles = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            if (roles == null)
            {
                return Result<List<string>>.Failure("Cannot find current user roles.");
            }
            return Result<List<string>>.Success(roles);
        }
    }
}
