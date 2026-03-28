namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerOverviewDto
{
    public PartnerDetailsDto? Partner { get; set; }
    public IReadOnlyCollection<PartnerNoteDto> RecentNotes { get; set; } = Array.Empty<PartnerNoteDto>();
    public IReadOnlyCollection<PartnerHistoryItemDto> RecentHistory { get; set; } = Array.Empty<PartnerHistoryItemDto>();
}