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