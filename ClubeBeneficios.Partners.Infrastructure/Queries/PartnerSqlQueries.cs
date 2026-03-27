namespace ClubeBeneficios.Partners.Infrastructure.Queries;

public static class PartnerSqlQueries
{
    public const string GetById = "SELECT TOP 1 * FROM dbo.vw_partners_admin_list WHERE id = @Id";
    public const string GetPending = "SELECT * FROM dbo.vw_partner_pending_details ORDER BY created_at DESC";

    public const string GetSummary = @"
                                        SELECT
                                            COUNT(1) AS TotalPartners,
                                            SUM(CASE WHEN status = 'active' THEN 1 ELSE 0 END) AS ActivePartners,
                                            SUM(CASE WHEN status = 'pending' THEN 1 ELSE 0 END) AS PendingPartners,
                                            SUM(CASE WHEN status = 'under_review' THEN 1 ELSE 0 END) AS UnderReviewPartners
                                        FROM dbo.partners";

    public const string GetPagedBase = @"SELECT * FROM dbo.vw_partners_admin_list";
}
