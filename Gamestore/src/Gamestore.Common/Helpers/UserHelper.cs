using System.Security.Claims;

namespace Gamestore.Common.Helpers;

public static class UserHelper
{
    public static Guid GetUserId(ClaimsPrincipal claims)
    {
        var userIdClaim = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userIdClaim != null && Guid.TryParse(userIdClaim, out var userId)
            ? userId : throw new UnauthorizedAccessException();
    }

    public static string GetUserName(ClaimsPrincipal claims)
    {
        var userNameClaim = claims.FindFirst(ClaimTypes.Name).Value;

        return userNameClaim;
    }
}
