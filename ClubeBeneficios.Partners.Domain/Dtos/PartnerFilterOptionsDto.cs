namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerFilterOptionsDto
{
    public IReadOnlyCollection<string> Statuses { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Levels { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Categories { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Segments { get; set; } = Array.Empty<string>();
}