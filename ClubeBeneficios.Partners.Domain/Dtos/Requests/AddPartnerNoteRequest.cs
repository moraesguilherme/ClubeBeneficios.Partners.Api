namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class AddPartnerNoteRequest
{
    public string? NoteType { get; set; } = "general";
    public string? Content { get; set; }
    public Guid? CreatedByUserId { get; set; }
}