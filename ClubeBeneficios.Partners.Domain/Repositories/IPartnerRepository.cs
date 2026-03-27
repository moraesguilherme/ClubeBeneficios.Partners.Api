using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Entities;

namespace ClubeBeneficios.Partners.Domain.Repositories;

public interface IPartnerRepository
{
    Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerPendingItemDto>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<PartnerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePartnerRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdatePartnerRequest request, CancellationToken cancellationToken = default);
    Task ChangeStatusAsync(Guid id, ChangePartnerStatusRequest request, CancellationToken cancellationToken = default);
    Task AddNoteAsync(Guid id, AddPartnerNoteRequest request, CancellationToken cancellationToken = default);
}
