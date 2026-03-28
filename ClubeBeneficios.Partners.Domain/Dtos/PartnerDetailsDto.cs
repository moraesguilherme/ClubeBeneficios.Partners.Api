namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDetailsDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public bool IndicationFlowEnabled { get; set; }
    public bool AccessCodeFlowEnabled { get; set; }
    public string? OriginType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? InactivatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
    public int BenefitsCount { get; set; }
    public int ConvertedClientsCount { get; set; }
    public int CampaignsCount { get; set; }
    public int RafflesCount { get; set; }
    public decimal? PerformanceScore { get; set; }
    public DateTime? MetricsRefreshedAt { get; set; }
}