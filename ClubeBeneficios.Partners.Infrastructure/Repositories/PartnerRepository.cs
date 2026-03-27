using System.Data;
using Dapper;
using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Infrastructure.Context;
using ClubeBeneficios.Partners.Infrastructure.Queries;

namespace ClubeBeneficios.Partners.Infrastructure.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PartnerRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> CreateAsync(CreatePartnerRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@TradeName", request.TradeName);
        parameters.Add("@LegalName", request.LegalName);
        parameters.Add("@Document", request.Document);
        parameters.Add("@Email", request.Email);
        parameters.Add("@Phone", request.Phone);
        parameters.Add("@LogoUrl", request.LogoUrl);
        parameters.Add("@Segment", request.Segment);
        parameters.Add("@Category", request.Category);
        parameters.Add("@ServiceRegion", request.ServiceRegion);
        parameters.Add("@Website", request.Website);
        parameters.Add("@Instagram", request.Instagram);
        parameters.Add("@Description", request.Description);
        parameters.Add("@Level", request.Level);
        parameters.Add("@IndicationFlowEnabled", request.IndicationFlowEnabled);
        parameters.Add("@AccessCodeFlowEnabled", request.AccessCodeFlowEnabled);
        parameters.Add("@OriginType", request.OriginType);
        parameters.Add("@Status", request.Status);
        parameters.Add("@ResponsibleName", request.ResponsibleName);
        parameters.Add("@ResponsibleRole", request.ResponsibleRole);
        parameters.Add("@ResponsibleEmail", request.ResponsibleEmail);
        parameters.Add("@ResponsiblePhone", request.ResponsiblePhone);
        parameters.Add("@CreatedByUserId", request.CreatedByUserId);
        parameters.Add("@InitialNote", request.InitialNote);

        return await connection.ExecuteScalarAsync<Guid>(
            "dbo.usp_partners_create",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Guid id, UpdatePartnerRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", id);
        parameters.Add("@TradeName", request.TradeName);
        parameters.Add("@LegalName", request.LegalName);
        parameters.Add("@Document", request.Document);
        parameters.Add("@Email", request.Email);
        parameters.Add("@Phone", request.Phone);
        parameters.Add("@LogoUrl", request.LogoUrl);
        parameters.Add("@Segment", request.Segment);
        parameters.Add("@Category", request.Category);
        parameters.Add("@ServiceRegion", request.ServiceRegion);
        parameters.Add("@Website", request.Website);
        parameters.Add("@Instagram", request.Instagram);
        parameters.Add("@Description", request.Description);
        parameters.Add("@Level", request.Level);
        parameters.Add("@ResponsibleName", request.ResponsibleName);
        parameters.Add("@ResponsibleRole", request.ResponsibleRole);
        parameters.Add("@ResponsibleEmail", request.ResponsibleEmail);
        parameters.Add("@ResponsiblePhone", request.ResponsiblePhone);

        await connection.ExecuteAsync(
            "dbo.usp_partners_update",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task ChangeStatusAsync(Guid id, ChangePartnerStatusRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", id);
        parameters.Add("@NewStatus", request.NewStatus);
        parameters.Add("@Reason", request.Reason);
        parameters.Add("@ChangedByUserId", request.ChangedByUserId);

        await connection.ExecuteAsync(
            "dbo.usp_partners_change_status",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task AddNoteAsync(Guid id, AddPartnerNoteRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", id);
        parameters.Add("@NoteType", request.NoteType);
        parameters.Add("@Content", request.Content);
        parameters.Add("@CreatedByUserId", request.CreatedByUserId);

        await connection.ExecuteAsync(
            "dbo.usp_partners_add_note",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<PartnerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<PartnerDetailsDto>(
            PartnerSqlQueries.GetById,
            new { Id = id });
    }

    public async Task<IReadOnlyCollection<PartnerPendingItemDto>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryAsync<PartnerPendingItemDto>(PartnerSqlQueries.GetPending);
        return result.ToList();
    }

    public async Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryFirstOrDefaultAsync<PartnerDashboardSummaryDto>(PartnerSqlQueries.GetSummary);
        return result ?? new PartnerDashboardSummaryDto();
    }

    public async Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"SELECT * FROM dbo.vw_partners_admin_list
WHERE (@Search IS NULL OR trade_name LIKE '%' + @Search + '%' OR category LIKE '%' + @Search + '%' OR service_region LIKE '%' + @Search + '%')
  AND (@Status IS NULL OR status = @Status)
  AND (@Level IS NULL OR level = @Level)
  AND (@Category IS NULL OR category = @Category)
ORDER BY trade_name
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT(1)
FROM dbo.vw_partners_admin_list
WHERE (@Search IS NULL OR trade_name LIKE '%' + @Search + '%' OR category LIKE '%' + @Search + '%' OR service_region LIKE '%' + @Search + '%')
  AND (@Status IS NULL OR status = @Status)
  AND (@Level IS NULL OR level = @Level)
  AND (@Category IS NULL OR category = @Category);";

        var parameters = new
        {
            filter.Search,
            filter.Status,
            filter.Level,
            filter.Category,
            Offset = (filter.Page - 1) * filter.PageSize,
            filter.PageSize
        };

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var items = (await multi.ReadAsync<PartnerListItemDto>()).ToList();
        var total = await multi.ReadFirstAsync<int>();

        return new PagedResultDto<PartnerListItemDto>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
}
