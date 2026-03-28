namespace ClubeBeneficios.Partners.Infrastructure.Queries;

public static class PartnerSqlQueries
{
    public const string GetById = 
        @"
        SELECT TOP 1
            *
        FROM dbo.vw_partners_admin_list
        WHERE id = @Id;
        ";

    public const string GetNotes = 
        @"
        SELECT
            id,
            partner_id,
            note_type,
            content,
            created_by_user_id,
            created_at
        FROM dbo.partner_notes
        WHERE partner_id = @PartnerId
        ORDER BY created_at DESC;
        ";

    public const string GetRecentNotes = 
        @"
        SELECT TOP (@Take)
            id,
            partner_id,
            note_type,
            content,
            created_by_user_id,
            created_at
        FROM dbo.partner_notes
        WHERE partner_id = @PartnerId
        ORDER BY created_at DESC;
        ";

    public const string GetHistory = 
        @"
        SELECT
            id,
            partner_id,
            from_status,
            to_status,
            reason,
            changed_by_user_id,
            changed_at
        FROM dbo.partner_status_history
        WHERE partner_id = @PartnerId
        ORDER BY changed_at DESC;
        ";

    public const string GetRecentHistory = 
        @"
        SELECT TOP (@Take)
            id,
            partner_id,
            from_status,
            to_status,
            reason,
            changed_by_user_id,
            changed_at
        FROM dbo.partner_status_history
        WHERE partner_id = @PartnerId
        ORDER BY changed_at DESC;
        ";

    public const string GetFilterOptions = 
        @"
        SELECT DISTINCT status FROM dbo.partners WHERE status IS NOT NULL ORDER BY status;
        SELECT DISTINCT level FROM dbo.partners WHERE level IS NOT NULL ORDER BY level;
        SELECT DISTINCT category FROM dbo.partners WHERE category IS NOT NULL ORDER BY category;
        SELECT DISTINCT segment FROM dbo.partners WHERE segment IS NOT NULL ORDER BY segment;
        ";
}