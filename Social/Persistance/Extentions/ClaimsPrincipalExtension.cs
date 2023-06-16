
using System.Security.Claims;

namespace Persistance.Extentions;

public static class ClaimsPrincipalExtension
{
    public static int? GetLoginUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return  int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public static string? GetLoginUserName(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirstValue(ClaimTypes.Name);
    }

    public static string? GetLoginUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirstValue(ClaimTypes.Email);
    }
}
