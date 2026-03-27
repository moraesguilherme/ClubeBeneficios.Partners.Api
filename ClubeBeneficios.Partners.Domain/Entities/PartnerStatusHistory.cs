namespace ClubeBeneficios.Partners.Domain.Entities;

public class PartnerStatusHistory
{
    public long Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? FromStatus { get; set; }
    public string? ToStatus { get; set; }
    public string? Reason { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
}
