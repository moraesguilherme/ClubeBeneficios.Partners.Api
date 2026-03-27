namespace ClubeBeneficios.Partners.Domain.Entities;

public class PartnerMetricsSnapshot
{
    public Guid PartnerId { get; set; }
    public int BenefitsCount { get; set; }
    public int ConvertedClientsCount { get; set; }
    public int CampaignsCount { get; set; }
    public int RafflesCount { get; set; }
    public decimal? PerformanceScore { get; set; }
    public DateTime RefreshedAt { get; set; }
}
