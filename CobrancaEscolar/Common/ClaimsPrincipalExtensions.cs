using System.Security.Claims;

namespace CobrancaEscolar.Common;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
        => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static string GetUsername(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Name)!;

    public static bool IsSuperuser(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == "is_superuser" && c.Value == "true");

    public static Guid? GetEscritorioId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue("escritorio_id");
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
