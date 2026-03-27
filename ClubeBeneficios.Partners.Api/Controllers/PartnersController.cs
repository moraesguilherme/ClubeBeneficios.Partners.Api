using Microsoft.AspNetCore.Mvc;
using ClubeBeneficios.Partners.Domain.Dtos.Filters;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[Route("api/partners")]
public class PartnersController : ControllerBase
{
    private readonly IPartnerService _partnerService;

    public PartnersController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PartnerFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetPagedAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetPendingAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetDashboardSummaryAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerService.GetByIdAsync(id, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartnerRequest request, CancellationToken cancellationToken)
    {
        var id = await _partnerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePartnerRequest request, CancellationToken cancellationToken)
    {
        await _partnerService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangePartnerStatusRequest request, CancellationToken cancellationToken)
    {
        await _partnerService.ChangeStatusAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddPartnerNoteRequest request, CancellationToken cancellationToken)
    {
        await _partnerService.AddNoteAsync(id, request, cancellationToken);
        return NoContent();
    }
}
