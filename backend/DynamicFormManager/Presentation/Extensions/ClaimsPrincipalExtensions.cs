using System.Security.Claims;
using Application.DTO.Requests.Auth;
using Application.Services.Jwt.Constants;

namespace Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email)?.ToLower() ?? string.Empty;
    }

    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var isUidClaimsExists = Guid.TryParse(user.FindFirstValue(CustomClaimTypes.UserId), out var id);
        return isUidClaimsExists ? id : Guid.Empty;
    }

    public static AuthorizedUserRequest GetUser(this ClaimsPrincipal user)
    {
        return new AuthorizedUserRequest
        {
            UserId = user.GetUserId()
        };
    }
}