namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerListItemDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
    public string? Level { get; set; }
    public string? Segment { get; set; }
    public string? ServiceRegion { get; set; }
    public int BenefitsCount { get; set; }
    public int ConvertedClientsCount { get; set; }
    public int CampaignsCount { get; set; }
    public int RafflesCount { get; set; }
    public decimal? PerformanceScore { get; set; }
}
