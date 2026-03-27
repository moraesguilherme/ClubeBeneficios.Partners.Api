namespace ClubeBeneficios.Partners.Domain.Dtos.Responses;

public class PartnerResponse
{
    public Guid Id { get; set; }
    public string TradeName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
