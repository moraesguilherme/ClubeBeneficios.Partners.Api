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

    public async Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Search", Normalize(filter.Search));
        parameters.Add("@Status", Normalize(filter.Status));
        parameters.Add("@Level", Normalize(filter.Level));
        parameters.Add("@Category", Normalize(filter.Category));
        parameters.Add("@Segment", Normalize(filter.Segment));
        parameters.Add("@SortBy", NormalizeSortBy(filter.SortBy));
        parameters.Add("@SortDirection", NormalizeSortDirection(filter.SortDirection));
        parameters.Add("@Page", filter.Page);
        parameters.Add("@PageSize", filter.PageSize);

        var command = new CommandDefinition(
            "dbo.usp_partners_admin_search",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var items = (await connection.QueryAsync<PartnerListItemDto>(command)).ToList();
        var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;

        return PagedResultDto<PartnerListItemDto>.Create(
            items,
            totalCount,
            filter.Page,
            filter.PageSize);
    }

    public async Task<PagedResultDto<PartnerListItemDto>> GetPendingAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Search", Normalize(filter.Search));
        parameters.Add("@Category", Normalize(filter.Category));
        parameters.Add("@Segment", Normalize(filter.Segment));
        parameters.Add("@SortBy", NormalizeSortBy(filter.SortBy));
        parameters.Add("@SortDirection", NormalizeSortDirection(filter.SortDirection));
        parameters.Add("@Page", filter.Page);
        parameters.Add("@PageSize", filter.PageSize);

        var command = new CommandDefinition(
            "dbo.usp_partners_pending_search",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var items = (await connection.QueryAsync<PartnerListItemDto>(command)).ToList();
        var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;

        return PagedResultDto<PartnerListItemDto>.Create(
            items,
            totalCount,
            filter.Page,
            filter.PageSize);
    }

    public async Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();

            var command = new CommandDefinition(
                "dbo.usp_partners_admin_summary",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<PartnerDashboardSummaryDto>(command)
                ?? new PartnerDashboardSummaryDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERRO GetDashboardSummaryAsync: " + ex);
            throw;
        }
    }

    public async Task<PartnerFilterOptionsDto> GetFilterOptionsAsync(
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            PartnerSqlQueries.GetFilterOptions,
            cancellationToken: cancellationToken);

        using var multi = await connection.QueryMultipleAsync(command);

        var statuses = (await multi.ReadAsync<string>()).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToArray();
        var levels = (await multi.ReadAsync<string>()).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToArray();
        var categories = (await multi.ReadAsync<string>()).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToArray();
        var segments = (await multi.ReadAsync<string>()).Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToArray();

        return new PartnerFilterOptionsDto
        {
            Statuses = statuses,
            Levels = levels,
            Categories = categories,
            Segments = segments
        };
    }

    public async Task<PartnerDetailsDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            PartnerSqlQueries.GetById,
            new { Id = id },
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<PartnerDetailsDto>(command);
    }

    public async Task<PartnerOverviewDto?> GetOverviewAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var partnerCommand = new CommandDefinition(
            PartnerSqlQueries.GetById,
            new { Id = id },
            cancellationToken: cancellationToken);

        var notesCommand = new CommandDefinition(
            PartnerSqlQueries.GetRecentNotes,
            new { PartnerId = id, Take = 5 },
            cancellationToken: cancellationToken);

        var historyCommand = new CommandDefinition(
            PartnerSqlQueries.GetRecentHistory,
            new { PartnerId = id, Take = 10 },
            cancellationToken: cancellationToken);

        var partner = await connection.QueryFirstOrDefaultAsync<PartnerDetailsDto>(partnerCommand);
        if (partner is null)
        {
            return null;
        }

        var notes = (await connection.QueryAsync<PartnerNoteDto>(notesCommand)).ToArray();
        var history = (await connection.QueryAsync<PartnerHistoryItemDto>(historyCommand)).ToArray();

        return new PartnerOverviewDto
        {
            Partner = partner,
            RecentNotes = notes,
            RecentHistory = history
        };
    }

    public async Task<IReadOnlyCollection<PartnerNoteDto>> GetNotesAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            PartnerSqlQueries.GetNotes,
            new { PartnerId = partnerId },
            cancellationToken: cancellationToken);

        return (await connection.QueryAsync<PartnerNoteDto>(command)).ToArray();
    }

    public async Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetHistoryAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            PartnerSqlQueries.GetHistory,
            new { PartnerId = partnerId },
            cancellationToken: cancellationToken);

        return (await connection.QueryAsync<PartnerHistoryItemDto>(command)).ToArray();
    }

    public async Task<Guid> CreateAsync(
        CreatePartnerRequest request,
        CancellationToken cancellationToken = default)
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

        var command = new CommandDefinition(
            "dbo.usp_partners_create",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        var id = await connection.ExecuteScalarAsync<Guid>(command);
        return id;
    }

    public async Task UpdateAsync(
        Guid id,
        UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
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

        var command = new CommandDefinition(
            "dbo.usp_partners_update",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }

    public async Task ChangeStatusAsync(
        Guid id,
        ChangePartnerStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", id);
        parameters.Add("@NewStatus", request.NewStatus);
        parameters.Add("@Reason", request.Reason);
        parameters.Add("@ChangedByUserId", request.ChangedByUserId);

        var command = new CommandDefinition(
            "dbo.usp_partners_change_status",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }

    public async Task AddNoteAsync(
        Guid id,
        AddPartnerNoteRequest request,
        CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@PartnerId", id);
        parameters.Add("@NoteType", request.NoteType);
        parameters.Add("@Content", request.Content);
        parameters.Add("@CreatedByUserId", request.CreatedByUserId);

        var command = new CommandDefinition(
            "dbo.usp_partners_add_note",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string NormalizeSortBy(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return "created_at";
        }

        return sortBy.Trim().ToLowerInvariant();
    }

    private static string NormalizeSortDirection(string? sortDirection)
    {
        return string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
            ? "asc"
            : "desc";
    }
}