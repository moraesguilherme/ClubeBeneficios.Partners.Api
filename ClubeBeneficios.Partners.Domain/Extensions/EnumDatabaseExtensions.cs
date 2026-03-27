using ClubeBeneficios.Partners.Domain.Enums;

namespace ClubeBeneficios.Partners.Domain.Extensions;

public static class EnumDatabaseExtensions
{
    public static string ToDatabaseValue(this PartnerStatus status)
    {
        return status switch
        {
            PartnerStatus.Pending => "pending",
            PartnerStatus.UnderReview => "under_review",
            PartnerStatus.Active => "active",
            PartnerStatus.Inactive => "inactive",
            PartnerStatus.Rejected => "rejected",
            PartnerStatus.Suspended => "suspended",
            PartnerStatus.Blocked => "blocked",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static string ToDatabaseValue(this PartnerNoteType noteType)
    {
        return noteType switch
        {
            PartnerNoteType.General => "general",
            PartnerNoteType.Commercial => "commercial",
            PartnerNoteType.Operational => "operational",
            PartnerNoteType.Approval => "approval",
            _ => throw new ArgumentOutOfRangeException(nameof(noteType), noteType, null)
        };
    }
}
