param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectPath
)

$ErrorActionPreference = "Stop"

function Ensure-Directory {
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) {
        New-Item -ItemType Directory -Path $Path -Force | Out-Null
    }
}

function Write-Utf8File {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Content
    )

    $dir = Split-Path -Parent $Path
    if ($dir) {
        Ensure-Directory -Path $dir
    }

    $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($Path, $Content.TrimStart("`r","`n"), $utf8NoBom)
    Write-Host "OK  $Path" -ForegroundColor Green
}

if (-not (Test-Path -LiteralPath $ProjectPath)) {
    throw "ProjectPath não encontrado: $ProjectPath"
}

$requiredDirs = @(
    "ClubeBeneficios.Partners.Api\Controllers",
    "ClubeBeneficios.Partners.Api\Extensions",
    "ClubeBeneficios.Partners.Domain\Dtos",
    "ClubeBeneficios.Partners.Domain\Dtos\Filters",
    "ClubeBeneficios.Partners.Domain\Dtos\Requests",
    "ClubeBeneficios.Partners.Domain\Interfaces",
    "ClubeBeneficios.Partners.Domain\Repositories",
    "ClubeBeneficios.Partners.Domain\Services",
    "ClubeBeneficios.Partners.Infrastructure\Repositories",
    "ClubeBeneficios.Partners.Infrastructure\Services",
    "ClubeBeneficios.Partners.Infrastructure\Security",
    "ClubeBeneficios.Partners.Infrastructure\Queries",
    "ClubeBeneficios.Partners.Infrastructure\DependencyInjection"
)

foreach ($dir in $requiredDirs) {
    Ensure-Directory -Path (Join-Path $ProjectPath $dir)
}

# =========================================================
# DOMAIN
# =========================================================

$pagedResultDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PagedResultDto<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }

    public static PagedResultDto<T> Create(
        IReadOnlyCollection<T> items,
        int totalCount,
        int page,
        int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasNextPage = totalPages > 0 && page < totalPages,
            HasPreviousPage = page > 1
        };
    }
}
'@

$partnerFilterDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos.Filters;

public class PartnerFilterDto
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Level { get; set; }
    public string? Category { get; set; }
    public string? Segment { get; set; }
    public string? ServiceRegion { get; set; }
    public string SortBy { get; set; } = "created_at";
    public string SortDirection { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
'@

$partnerListItemDto = @'
using System.Text.Json.Serialization;

namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerListItemDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public bool IndicationFlowEnabled { get; set; }
    public bool AccessCodeFlowEnabled { get; set; }
    public string? OriginType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? InactivatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
    public int BenefitsCount { get; set; }
    public int ConvertedClientsCount { get; set; }
    public int CampaignsCount { get; set; }
    public int RafflesCount { get; set; }
    public decimal? PerformanceScore { get; set; }
    public DateTime? MetricsRefreshedAt { get; set; }

    [JsonIgnore]
    public int TotalCount { get; set; }
}
'@

$partnerDetailsDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDetailsDto
{
    public Guid Id { get; set; }
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public bool IndicationFlowEnabled { get; set; }
    public bool AccessCodeFlowEnabled { get; set; }
    public string? OriginType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public DateTime? InactivatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
    public int BenefitsCount { get; set; }
    public int ConvertedClientsCount { get; set; }
    public int CampaignsCount { get; set; }
    public int RafflesCount { get; set; }
    public decimal? PerformanceScore { get; set; }
    public DateTime? MetricsRefreshedAt { get; set; }
}
'@

$partnerNoteDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerNoteDto
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? NoteType { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
'@

$partnerHistoryItemDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerHistoryItemDto
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public string? FromStatus { get; set; }
    public string? ToStatus { get; set; }
    public string? Reason { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
}
'@

$partnerOverviewDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerOverviewDto
{
    public PartnerDetailsDto? Partner { get; set; }
    public IReadOnlyCollection<PartnerNoteDto> RecentNotes { get; set; } = Array.Empty<PartnerNoteDto>();
    public IReadOnlyCollection<PartnerHistoryItemDto> RecentHistory { get; set; } = Array.Empty<PartnerHistoryItemDto>();
}
'@

$partnerFilterOptionsDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerFilterOptionsDto
{
    public IReadOnlyCollection<string> Statuses { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Levels { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Categories { get; set; } = Array.Empty<string>();
    public IReadOnlyCollection<string> Segments { get; set; } = Array.Empty<string>();
}
'@

$partnerDashboardSummaryDto = @'
namespace ClubeBeneficios.Partners.Domain.Dtos;

public class PartnerDashboardSummaryDto
{
    public int TotalPartners { get; set; }
    public int ActivePartners { get; set; }
    public int PendingReviewPartners { get; set; }
    public int UnderReviewPartners { get; set; }
    public int ApprovedPartners { get; set; }
    public int InactivePartners { get; set; }
    public int RejectedPartners { get; set; }
    public int SuspendedPartners { get; set; }
    public int BlockedPartners { get; set; }
    public int BronzeCount { get; set; }
    public int SilverCount { get; set; }
    public int GoldCount { get; set; }
    public int DiamondCount { get; set; }
    public int PlatinumCount { get; set; }
}
'@

$createPartnerRequest = @'
namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class CreatePartnerRequest
{
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public bool IndicationFlowEnabled { get; set; } = true;
    public bool AccessCodeFlowEnabled { get; set; } = true;
    public string? OriginType { get; set; } = "admin_created";
    public string? Status { get; set; } = "active";
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public string? InitialNote { get; set; }
}
'@

$updatePartnerRequest = @'
namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class UpdatePartnerRequest
{
    public string? TradeName { get; set; }
    public string? LegalName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
    public string? ServiceRegion { get; set; }
    public string? Website { get; set; }
    public string? Instagram { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public string? ResponsibleName { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? ResponsibleEmail { get; set; }
    public string? ResponsiblePhone { get; set; }
}
'@

$changePartnerStatusRequest = @'
namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class ChangePartnerStatusRequest
{
    public string? NewStatus { get; set; }
    public string? Reason { get; set; }
    public Guid? ChangedByUserId { get; set; }
}
'@

$addPartnerNoteRequest = @'
namespace ClubeBeneficios.Partners.Domain.Dtos.Requests;

public class AddPartnerNoteRequest
{
    public string? NoteType { get; set; } = "internal";
    public string? Content { get; set; }
    public Guid? CreatedByUserId { get; set; }
}
'@

$iUserContext = @'
namespace ClubeBeneficios.Partners.Domain.Interfaces;

public interface IUserContext
{
    Guid? UserId { get; }
    Guid? PartnerId { get; }
    string? Role { get; }
    Guid RequirePartnerId();
}
'@

$iPartnerRepository = @'
using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;

namespace ClubeBeneficios.Partners.Domain.Repositories;

public interface IPartnerRepository
{
    Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default);
    Task<PagedResultDto<PartnerListItemDto>> GetPendingAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default);
    Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<PartnerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PartnerOverviewDto?> GetOverviewAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerNoteDto>> GetNotesAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetHistoryAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<PartnerFilterOptionsDto> GetFilterOptionsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePartnerRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdatePartnerRequest request, CancellationToken cancellationToken = default);
    Task ChangeStatusAsync(Guid id, ChangePartnerStatusRequest request, CancellationToken cancellationToken = default);
    Task AddNoteAsync(Guid id, AddPartnerNoteRequest request, CancellationToken cancellationToken = default);
}
'@

$iPartnerService = @'
using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;

namespace ClubeBeneficios.Partners.Domain.Services;

public interface IPartnerService
{
    Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default);
    Task<PagedResultDto<PartnerListItemDto>> GetPendingAsync(PartnerFilterDto filter, CancellationToken cancellationToken = default);
    Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<PartnerFilterOptionsDto> GetFilterOptionsAsync(CancellationToken cancellationToken = default);
    Task<PartnerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PartnerOverviewDto?> GetOverviewAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerNoteDto>> GetNotesAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetHistoryAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePartnerRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdatePartnerRequest request, CancellationToken cancellationToken = default);
    Task ChangeStatusAsync(Guid id, ChangePartnerStatusRequest request, CancellationToken cancellationToken = default);
    Task AddNoteAsync(Guid id, AddPartnerNoteRequest request, CancellationToken cancellationToken = default);

    Task<PartnerDetailsDto?> GetMyPartnerAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<PartnerOverviewDto?> GetMyOverviewAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerNoteDto>> GetMyNotesAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetMyHistoryAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task UpdateMyPartnerAsync(Guid partnerId, UpdatePartnerRequest request, CancellationToken cancellationToken = default);
}
'@

# =========================================================
# INFRASTRUCTURE
# =========================================================

$partnerSqlQueries = @'
namespace ClubeBeneficios.Partners.Infrastructure.Queries;

public static class PartnerSqlQueries
{
    public const string GetById = @"
SELECT TOP 1
    *
FROM dbo.vw_partners_admin_list
WHERE id = @Id;
";

    public const string GetNotes = @"
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

    public const string GetRecentNotes = @"
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

    public const string GetHistory = @"
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

    public const string GetRecentHistory = @"
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

    public const string GetFilterOptions = @"
SELECT DISTINCT status FROM dbo.partners WHERE status IS NOT NULL ORDER BY status;
SELECT DISTINCT level FROM dbo.partners WHERE level IS NOT NULL ORDER BY level;
SELECT DISTINCT category FROM dbo.partners WHERE category IS NOT NULL ORDER BY category;
SELECT DISTINCT segment FROM dbo.partners WHERE segment IS NOT NULL ORDER BY segment;
";
}
'@

$currentUserContext = @'
using System.Security.Claims;
using ClubeBeneficios.Partners.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClubeBeneficios.Partners.Infrastructure.Security;

public class CurrentUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId => TryGetGuid(
        FindClaim(ClaimTypes.NameIdentifier) ??
        FindClaim("sub"));

    public Guid? PartnerId => TryGetGuid(
        FindClaim("partner_id") ??
        FindClaim("partnerId"));

    public string? Role =>
        FindClaim(ClaimTypes.Role) ??
        FindClaim("role");

    public Guid RequirePartnerId()
    {
        var partnerId = PartnerId;
        if (!partnerId.HasValue)
        {
            throw new UnauthorizedAccessException("O token autenticado não possui partner_id.");
        }

        return partnerId.Value;
    }

    private string? FindClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }

    private static Guid? TryGetGuid(string? value)
    {
        return Guid.TryParse(value, out var parsed)
            ? parsed
            : null;
    }
}
'@

$partnerRepository = @'
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
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "dbo.usp_partners_admin_summary",
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<PartnerDashboardSummaryDto>(command)
            ?? new PartnerDashboardSummaryDto();
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
'@

$partnerService = @'
using ClubeBeneficios.Partners.Domain.Dtos;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _partnerRepository;

    public PartnerService(IPartnerRepository partnerRepository)
    {
        _partnerRepository = partnerRepository;
    }

    public Task<PagedResultDto<PartnerListItemDto>> GetPagedAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetPagedAsync(filter, cancellationToken);

    public Task<PagedResultDto<PartnerListItemDto>> GetPendingAsync(
        PartnerFilterDto filter,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetPendingAsync(filter, cancellationToken);

    public Task<PartnerDashboardSummaryDto> GetDashboardSummaryAsync(
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetDashboardSummaryAsync(cancellationToken);

    public Task<PartnerFilterOptionsDto> GetFilterOptionsAsync(
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetFilterOptionsAsync(cancellationToken);

    public Task<PartnerDetailsDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetByIdAsync(id, cancellationToken);

    public Task<PartnerOverviewDto?> GetOverviewAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetOverviewAsync(id, cancellationToken);

    public Task<IReadOnlyCollection<PartnerNoteDto>> GetNotesAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetNotesAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetHistoryAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetHistoryAsync(partnerId, cancellationToken);

    public Task<Guid> CreateAsync(
        CreatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.CreateAsync(request, cancellationToken);

    public Task UpdateAsync(
        Guid id,
        UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.UpdateAsync(id, request, cancellationToken);

    public Task ChangeStatusAsync(
        Guid id,
        ChangePartnerStatusRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.ChangeStatusAsync(id, request, cancellationToken);

    public Task AddNoteAsync(
        Guid id,
        AddPartnerNoteRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.AddNoteAsync(id, request, cancellationToken);

    public Task<PartnerDetailsDto?> GetMyPartnerAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetByIdAsync(partnerId, cancellationToken);

    public Task<PartnerOverviewDto?> GetMyOverviewAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetOverviewAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerNoteDto>> GetMyNotesAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetNotesAsync(partnerId, cancellationToken);

    public Task<IReadOnlyCollection<PartnerHistoryItemDto>> GetMyHistoryAsync(
        Guid partnerId,
        CancellationToken cancellationToken = default)
        => _partnerRepository.GetHistoryAsync(partnerId, cancellationToken);

    public Task UpdateMyPartnerAsync(
        Guid partnerId,
        UpdatePartnerRequest request,
        CancellationToken cancellationToken = default)
        => _partnerRepository.UpdateAsync(partnerId, request, cancellationToken);
}
'@

$infraServiceCollectionExtensions = @'
using ClubeBeneficios.Partners.Domain.Interfaces;
using ClubeBeneficios.Partners.Domain.Repositories;
using ClubeBeneficios.Partners.Domain.Services;
using ClubeBeneficios.Partners.Infrastructure.Context;
using ClubeBeneficios.Partners.Infrastructure.Repositories;
using ClubeBeneficios.Partners.Infrastructure.Security;
using ClubeBeneficios.Partners.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClubeBeneficios.Partners.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<DbConnectionFactory>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IUserContext, CurrentUserContext>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPartnerService, PartnerService>();
        return services;
    }
}
'@

# =========================================================
# API
# =========================================================

$authorizationExtensions = @'
namespace ClubeBeneficios.Partners.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            options.AddPolicy("PartnerOnly", policy => policy.RequireRole("partner"));
            options.AddPolicy("PartnerOrAdmin", policy => policy.RequireRole("partner", "admin"));
        });

        return services;
    }
}
'@

$startupCs = @'
using ClubeBeneficios.Partners.Api.Extensions;
using ClubeBeneficios.Partners.Infrastructure.DependencyInjection;
using Dapper;

namespace ClubeBeneficios.Partners.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddApiControllers();
        services.AddApiSwagger();
        services.AddApiCors();
        services.AddApiAuthentication(Configuration);
        services.AddApiAuthorization();
        services.AddInfrastructure();
        services.AddApplicationServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseApiExceptionHandling();
        app.UseApiSwagger();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("DefaultPolicy");
        app.UseAuthentication();
        app.UseUserContext();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
'@

$partnersController = @'
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Interfaces;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[Authorize(Policy = "AdminOnly")]
[Route("api/admin/partners")]
[Route("api/partners")]
public class PartnersController : ControllerBase
{
    private readonly IPartnerService _partnerService;
    private readonly IUserContext _userContext;

    public PartnersController(
        IPartnerService partnerService,
        IUserContext userContext)
    {
        _partnerService = partnerService;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PartnerFilterDto filter,
        CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetPagedAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(
        [FromQuery] PartnerFilterDto filter,
        CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetPendingAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetDashboardSummaryAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("filter-options")]
    public async Task<IActionResult> GetFilterOptions(CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetFilterOptionsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetByIdAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/overview")]
    public async Task<IActionResult> GetOverview(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetOverviewAsync(id, cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/notes")]
    public async Task<IActionResult> GetNotes(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetNotesAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetHistoryAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePartnerRequest request,
        CancellationToken cancellationToken)
    {
        request.CreatedByUserId ??= _userContext.UserId;
        request.Status ??= "active";
        request.OriginType ??= "admin_created";

        var id = await _partnerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePartnerRequest request,
        CancellationToken cancellationToken)
    {
        await _partnerService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        request.ChangedByUserId ??= _userContext.UserId;
        await _partnerService.ChangeStatusAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        var statusRequest = CreateStatusRequest("approved", request.Reason);
        await _partnerService.ChangeStatusAsync(id, statusRequest, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        var statusRequest = CreateStatusRequest("rejected", request.Reason);
        await _partnerService.ChangeStatusAsync(id, statusRequest, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        var statusRequest = CreateStatusRequest("active", request.Reason);
        await _partnerService.ChangeStatusAsync(id, statusRequest, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        var statusRequest = CreateStatusRequest("inactive", request.Reason);
        await _partnerService.ChangeStatusAsync(id, statusRequest, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<IActionResult> Suspend(
        Guid id,
        [FromBody] ChangePartnerStatusRequest request,
        CancellationToken cancellationToken)
    {
        var statusRequest = CreateStatusRequest("suspended", request.Reason);
        await _partnerService.ChangeStatusAsync(id, statusRequest, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(
        Guid id,
        [FromBody] AddPartnerNoteRequest request,
        CancellationToken cancellationToken)
    {
        request.CreatedByUserId ??= _userContext.UserId;
        await _partnerService.AddNoteAsync(id, request, cancellationToken);
        return NoContent();
    }

    private ChangePartnerStatusRequest CreateStatusRequest(string newStatus, string? reason)
    {
        return new ChangePartnerStatusRequest
        {
            NewStatus = newStatus,
            Reason = reason,
            ChangedByUserId = _userContext.UserId
        };
    }
}
'@

$partnerMeController = @'
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Interfaces;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[Authorize(Policy = "PartnerOnly")]
[Route("api/partner/me")]
public class PartnerMeController : ControllerBase
{
    private readonly IPartnerService _partnerService;
    private readonly IUserContext _userContext;

    public PartnerMeController(
        IPartnerService partnerService,
        IUserContext userContext)
    {
        _partnerService = partnerService;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();
        var result = await _partnerService.GetMyPartnerAsync(partnerId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview(CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();
        var result = await _partnerService.GetMyOverviewAsync(partnerId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("notes")]
    public async Task<IActionResult> GetNotes(CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();
        var result = await _partnerService.GetMyNotesAsync(partnerId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();
        var result = await _partnerService.GetMyHistoryAsync(partnerId, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdatePartnerRequest request,
        CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();
        await _partnerService.UpdateMyPartnerAsync(partnerId, request, cancellationToken);
        return NoContent();
    }
}
'@

# =========================================================
# WRITE FILES
# =========================================================

Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PagedResultDto.cs") -Content $pagedResultDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\Filters\PartnerFilterDto.cs") -Content $partnerFilterDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerListItemDto.cs") -Content $partnerListItemDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerDetailsDto.cs") -Content $partnerDetailsDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerNoteDto.cs") -Content $partnerNoteDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerHistoryItemDto.cs") -Content $partnerHistoryItemDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerOverviewDto.cs") -Content $partnerOverviewDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerFilterOptionsDto.cs") -Content $partnerFilterOptionsDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\PartnerDashboardSummaryDto.cs") -Content $partnerDashboardSummaryDto
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\Requests\CreatePartnerRequest.cs") -Content $createPartnerRequest
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\Requests\UpdatePartnerRequest.cs") -Content $updatePartnerRequest
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\Requests\ChangePartnerStatusRequest.cs") -Content $changePartnerStatusRequest
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Dtos\Requests\AddPartnerNoteRequest.cs") -Content $addPartnerNoteRequest
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Interfaces\IUserContext.cs") -Content $iUserContext
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Repositories\IPartnerRepository.cs") -Content $iPartnerRepository
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Domain\Services\IPartnerService.cs") -Content $iPartnerService

Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Infrastructure\Queries\PartnerSqlQueries.cs") -Content $partnerSqlQueries
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Infrastructure\Security\CurrentUserContext.cs") -Content $currentUserContext
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Infrastructure\Repositories\PartnerRepository.cs") -Content $partnerRepository
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Infrastructure\Services\PartnerService.cs") -Content $partnerService
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Infrastructure\DependencyInjection\ServiceCollectionExtensions.cs") -Content $infraServiceCollectionExtensions

Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Api\Extensions\AuthorizationExtensions.cs") -Content $authorizationExtensions
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Api\Startup.cs") -Content $startupCs
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Api\Controllers\PartnersController.cs") -Content $partnersController
Write-Utf8File -Path (Join-Path $ProjectPath "ClubeBeneficios.Partners.Api\Controllers\PartnerMeController.cs") -Content $partnerMeController

Write-Host ""
Write-Host "Backend de parceiros atualizado com sucesso." -ForegroundColor Cyan
Write-Host "Próximo passo recomendado:" -ForegroundColor Cyan
Write-Host "1) dotnet restore" -ForegroundColor Yellow
Write-Host "2) dotnet build" -ForegroundColor Yellow
Write-Host "3) revisar git diff" -ForegroundColor Yellow