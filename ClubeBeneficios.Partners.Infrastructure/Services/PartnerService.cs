using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _partnerRepository;

    public PartnerService(IPartnerRepository partnerRepository)
    {
        _partnerRepository = partnerRepository;
    }

    public Task<Guid> CreateAsync(CreatePartnerRequest request, CancellationToken cancellationToken = default)
        => _partnerRepository.CreateAsync(request, cancellationToken);

    public Task UpdateAsync(Guid id, UpdatePartnerRequest request, CancellationToken cancellationToken = default)
        => _partnerRepository.UpdateAsync(id, request, cancellationToken);

    public Task ChangeStatusAsync(Guid id, ChangePartnerStatusRequest request, CancellationToken cancellationToken = default)
        => _partnerRepository.ChangeStatusAsync(id, request, cancellationToken);

    public Task AddNoteAsync(Guid id, AddPartnerNoteRequest request, CancellationToken cancellationToken = default)
        => _partnerRepository.AddNoteAsync(id, request, cancellationToken);

    public Task<PartnerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _partnerRepository.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyCollection<PartnerPendingItemDto>> GetPendingAsync(CancellationToken cancellationToken = default)
        => _partnerRepository.GetPendingAsync(cancellationToken);

    public Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        => _partnerRepository.GetDashboardSummaryAsync(cancellationToken);

    public Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default)
        => _partnerRepository.GetPagedAsync(filter, cancellationToken);
}
