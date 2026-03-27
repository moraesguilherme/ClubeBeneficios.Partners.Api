namespace ClubeBeneficios.Partners.Domain.Entities;

public class PartnerNote
{
    public long Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? NoteType { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
