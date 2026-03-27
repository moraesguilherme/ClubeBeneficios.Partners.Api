namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDto
{
    public Guid Id { get; set; }
    public string TradeName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Level { get; set; }
}
