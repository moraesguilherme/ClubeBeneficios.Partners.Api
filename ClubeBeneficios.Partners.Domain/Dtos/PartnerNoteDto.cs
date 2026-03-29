namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerNoteDto
{
    public long Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? NoteType { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}