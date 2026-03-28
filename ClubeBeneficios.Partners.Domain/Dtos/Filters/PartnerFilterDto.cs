namespace ClubeBeneficios.Partners.Domain.Dtos.Filters;

public class PartnerFilterDto
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Level { get; set; }
    public string? Category { get; set; }
    public string? Segment { get; set; }
    public string? ServiceRegion { get; set; }
    public string SortBy { get; set; } = "created_at";
    public string SortDirection { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}