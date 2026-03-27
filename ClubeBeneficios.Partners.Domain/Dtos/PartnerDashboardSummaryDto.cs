namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDashboardSummaryDto
{
    public int TotalPartners { get; set; }
    public int ActivePartners { get; set; }
    public int PendingPartners { get; set; }
    public int UnderReviewPartners { get; set; }
}
