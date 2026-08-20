using System.ComponentModel.DataAnnotations;

namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class CreatePartnerAccessRequest
{
    [Required(ErrorMessage = "Informe o nome da empresa.")]
    public string? TradeName { get; set; }

    public string? LegalName { get; set; }

    public string? Document { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    [Required(ErrorMessage = "Informe o segmento da empresa.")]
    public string? Segment { get; set; }

    [Required(ErrorMessage = "Informe a categoria da empresa.")]
    public string? Category { get; set; }

    public string? ServiceRegion { get; set; }

    public string? Website { get; set; }

    public string? Instagram { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Informe o nome do responsável.")]
    public string? ResponsibleName { get; set; }

    public string? ResponsibleRole { get; set; }

    [Required(ErrorMessage = "Informe o e-mail do responsável.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string? ResponsibleEmail { get; set; }

    [Required(ErrorMessage = "Informe o telefone/WhatsApp do responsável.")]
    public string? ResponsiblePhone { get; set; }

    public string? InitialNote { get; set; }
}