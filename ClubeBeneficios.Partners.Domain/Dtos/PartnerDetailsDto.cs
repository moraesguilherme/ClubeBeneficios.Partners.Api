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
    public string? Category { get; set; }
    public string? Level { get; set; }
    public string? Segment { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool IndicationFlowEnabled { get; set; }
    public bool AccessCodeFlowEnabled { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
}
