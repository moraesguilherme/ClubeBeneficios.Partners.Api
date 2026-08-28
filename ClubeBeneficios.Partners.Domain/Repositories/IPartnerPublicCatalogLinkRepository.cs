using ClubeBeneficios.Partners.Domain.Dtos;

namespace ClubeBeneficios.Partners.Domain.Repositories;

public interface IPartnerPublicCatalogLinkRepository
{
    Task<PartnerPublicCatalogLinkDto?> GetOrCreateAsync(
        Guid partnerId,
        Guid? createdByUserId,
        CancellationToken cancellationToken = default);
}