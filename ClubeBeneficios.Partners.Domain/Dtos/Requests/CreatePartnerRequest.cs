namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class CreatePartnerRequest
{
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public bool IndicationFlowEnabled { get; set; } = true;
    public bool AccessCodeFlowEnabled { get; set; } = true;
    public string? OriginType { get; set; } = "admin_created";
    public string? Status { get; set; } = "active";
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public string? InitialNote { get; set; }
}