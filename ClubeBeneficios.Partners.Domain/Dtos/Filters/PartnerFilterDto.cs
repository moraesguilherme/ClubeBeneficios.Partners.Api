namespace ClubeBeneficios.Partners.Domain.Dtos.Filters;

public class PartnerFilterDto
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Level { get; set; }
    public string? Category { get; set; }
    public string SortBy { get; set; } = "name";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
