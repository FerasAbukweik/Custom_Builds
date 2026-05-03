using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Custom_Builds.Core.Services.CurrUserServices
{
    public class GetCurrUserService : IGetCurrUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<Guid> GetTargetUserId(Guid? suppliedId)
        {
            // if no id is supplied, return current user id
            if (suppliedId == null)
            {
                // get curr user id and check if success
                var getCurrUserIdResult = GetUserId();
                if (!getCurrUserIdResult.IsSuccess)
                {
                    return Result<Guid>.Failure("Cannot get current user id");
                }

                // return current user id
                return Result<Guid>.Success(getCurrUserIdResult.Value!);
            }
            else
            {
                // if value is supplied and its not admin , forbid the action
                if (!_httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Any(r => r.Value == RoleEnums.Admin.ToString()) ?? false)
                {
                    return Result<Guid>.Failure("Only admin can delete other users");
                }
            }

            // this if admin and id is supplied, return the supplied id
            return Result<Guid>.Success(suppliedId.Value);
        }
        public Result<Guid> GetUserId()
        {
            // get current user id from claim
            string? userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

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
            bool? isAdmin = _httpContextAccessor.HttpContext?.User.IsInRole(RoleEnums.Admin.ToString());
            if (isAdmin == null)
            {
                // if cannt determine user role , return failure result
                return Result<bool>.Failure("Failed to determine user role.", HttpStatusCode.InternalServerError);
            }

            return Result<bool>.Success(isAdmin.Value);
        }
    }
}
