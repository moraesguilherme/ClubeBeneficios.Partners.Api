using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Infrastructure.Services;

public class PartnerPublicCatalogLinkService : IPartnerPublicCatalogLinkService
{
    private readonly IPartnerPublicCatalogLinkRepository _repository;

    public PartnerPublicCatalogLinkService(IPartnerPublicCatalogLinkRepository repository)
    {
        _repository = repository;
    }

    public Task<PartnerPublicCatalogLinkDto?> GetOrCreateMyLinkAsync(
        Guid partnerId,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetOrCreateAsync(
            partnerId,
            userId,
            cancellationToken);
    }
}