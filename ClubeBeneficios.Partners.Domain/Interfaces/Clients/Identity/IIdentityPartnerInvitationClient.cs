namespace ClubeBeneficios.Partners.Infrastructure.Clients.Identity;

public interface IIdentityPartnerInvitationClient
{
    Task<PartnerInvitationCreatedResponse?> CreateInvitationAsync(
        Guid partnerId,
        Guid? createdByUserId,
        CancellationToken cancellationToken = default);
}