using ClubeBeneficios.Partners.Domain.Dtos;

namespace ClubeBeneficios.Partners.Domain.Services;

public interface IPartnerPublicCatalogLinkService
{
    Task<PartnerPublicCatalogLinkDto?> GetOrCreateMyLinkAsync(
        Guid partnerId,
        Guid? userId,
        CancellationToken cancellationToken = default);
}