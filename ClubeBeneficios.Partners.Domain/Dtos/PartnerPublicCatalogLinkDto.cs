namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerPublicCatalogLinkDto
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? PartnerName { get; set; }

    public string? Slug { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}