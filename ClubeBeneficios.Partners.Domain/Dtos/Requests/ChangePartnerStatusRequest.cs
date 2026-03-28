namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class ChangePartnerStatusRequest
{
    public string? NewStatus { get; set; }
    public string? Reason { get; set; }
    public Guid? ChangedByUserId { get; set; }
}