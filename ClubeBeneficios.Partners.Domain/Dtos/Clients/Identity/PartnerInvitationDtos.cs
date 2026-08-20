namespace ClubeBeneficios.Partners.Infrastructure.Clients.Identity;

public class CreateInternalPartnerInvitationRequest
{
    public Guid PartnerId { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public int ExpirationDays { get; set; }
    public string? ActivationBaseUrl { get; set; }
}

public class PartnerInvitationCreatedResponse
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public Guid? UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public string ActivationUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsiblePhone { get; set; }
}