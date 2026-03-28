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

    public Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetPagedAsync(filter, cancellationToken);

    public Task<PagedResultDto<PartnerListItemDto>> GetPendingAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetPendingAsync(filter, cancellationToken);

    public Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetDashboardSummaryAsync(cancellationToken);

    public Task<PartnerFilterOptionsDto> GetFilterOptionsAsync(
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetFilterOptionsAsync(cancellationToken);

    public Task<PartnerDetailsDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetByIdAsync(id, cancellationToken);

    public Task<PartnerOverviewDto?> GetOverviewAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetOverviewAsync(id, cancellationToken);

    public Task<IReadOnlyCollection<PartnerNoteDto>> GetNotesAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetNotesAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetHistoryAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetHistoryAsync(partnerId, cancellationToken);

    public Task<Guid> CreateAsync(
        CreatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.CreateAsync(request, cancellationToken);

    public Task UpdateAsync(
        Guid id,
        UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.UpdateAsync(id, request, cancellationToken);

    public Task ChangeStatusAsync(
        Guid id,
        ChangePartnerStatusRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.ChangeStatusAsync(id, request, cancellationToken);

    public Task AddNoteAsync(
        Guid id,
        AddPartnerNoteRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.AddNoteAsync(id, request, cancellationToken);

    public Task<PartnerDetailsDto?> GetMyPartnerAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetByIdAsync(partnerId, cancellationToken);

    public Task<PartnerOverviewDto?> GetMyOverviewAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetOverviewAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerNoteDto>> GetMyNotesAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetNotesAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetMyHistoryAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetHistoryAsync(partnerId, cancellationToken);

    public Task UpdateMyPartnerAsync(
        Guid partnerId,
        UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.UpdateAsync(partnerId, request, cancellationToken);
}