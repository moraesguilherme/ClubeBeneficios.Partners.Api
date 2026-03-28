using System.Security.Claims;
using ClubeBeneficios.Partners.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClubeBeneficios.Partners.Infrastructure.Security;

public class CurrentUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => TryGetGuid(
        FindClaim(ClaimTypes.NameIdentifier) ??
        FindClaim("sub"));

    public Guid? PartnerId => TryGetGuid(
        FindClaim("partner_id") ??
        FindClaim("partnerId"));

    public string? Role =>
        FindClaim(ClaimTypes.Role) ??
        FindClaim("role");

    public Guid RequirePartnerId()
    {
        var partnerId = PartnerId;
        if (!partnerId.HasValue)
        {
            throw new UnauthorizedAccessException("O token autenticado não possui partner_id.");
        }

        return partnerId.Value;
    }

    private string? FindClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?
            .User?
            .Claims?
            .FirstOrDefault(c => c.Type == claimType)?
            .Value;
    }

    private static Guid? TryGetGuid(string? value)
    {
        return Guid.TryParse(value, out var parsed)
            ? parsed
            : null;
    }
}