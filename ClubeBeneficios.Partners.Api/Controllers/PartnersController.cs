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