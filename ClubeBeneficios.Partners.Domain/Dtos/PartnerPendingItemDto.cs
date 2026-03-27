namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerPendingItemDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleEmail { get; set; }
    public DateTime CreatedAt { get; set; }
}
