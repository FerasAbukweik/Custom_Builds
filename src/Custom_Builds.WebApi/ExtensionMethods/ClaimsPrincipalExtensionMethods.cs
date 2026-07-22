
using System.Security.Claims;
using Custom_Builds.Core.Common;

namespace custom_Peripherals.ExtensionMethods;

public static class ClaimsPrincipalExtensionMethods
{
    public static Result<Guid> GetId(this ClaimsPrincipal claims)
    {
        var idString = claims.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(idString))
            return Result<Guid>.Failure("user id not found in claims");

        if (!Guid.TryParse(idString, out var userId))
            return Result<Guid>.Failure("bad user id formate");

        return Result<Guid>.Success(userId);
    }
}