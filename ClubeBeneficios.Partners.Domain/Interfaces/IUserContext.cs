namespace ClubeBeneficios.Partners.Domain.Interfaces;

public interface IUserContext
{
    Guid? UserId { get; }
    Guid? PartnerId { get; }
    string? Role { get; }
    Guid RequirePartnerId();
}