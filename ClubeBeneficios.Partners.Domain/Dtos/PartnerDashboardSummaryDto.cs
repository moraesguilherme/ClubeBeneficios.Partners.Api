namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDashboardSummaryDto
{
    public int TotalPartners { get; set; }
    public int ActivePartners { get; set; }
    public int PendingReviewPartners { get; set; }
    public int UnderReviewPartners { get; set; }
    public int ApprovedPartners { get; set; }
    public int InactivePartners { get; set; }
    public int RejectedPartners { get; set; }
    public int SuspendedPartners { get; set; }
    public int BlockedPartners { get; set; }
    public int BronzeCount { get; set; }
    public int SilverCount { get; set; }
    public int GoldCount { get; set; }
    public int DiamondCount { get; set; }
    public int PlatinumCount { get; set; }
}