using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubeBeneficios.Partners.Domain.Interfaces;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[Authorize(Policy = "PartnerOnly")]
[Route("api/partner/public-catalog-link")]
public class PartnerPublicCatalogLinksController : ControllerBase
{
    private readonly IPartnerPublicCatalogLinkService _service;
    private readonly IUserContext _userContext;
    private readonly IConfiguration _configuration;

    public PartnerPublicCatalogLinksController(
        IPartnerPublicCatalogLinkService service,
        IUserContext userContext,
        IConfiguration configuration)
    {
        _service = service;
        _userContext = userContext;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrCreate(CancellationToken cancellationToken)
    {
        var partnerId = _userContext.RequirePartnerId();

        var result = await _service.GetOrCreateMyLinkAsync(
            partnerId,
            _userContext.UserId,
            cancellationToken);

        if (result is null)
            return NotFound();

        var publicBaseUrl = _configuration["PublicCatalog:BaseUrl"];

        var publicUrl = !string.IsNullOrWhiteSpace(publicBaseUrl) && !string.IsNullOrWhiteSpace(result.Slug)
            ? $"{publicBaseUrl.TrimEnd('/')}/{result.Slug}"
            : null;

        return Ok(new
        {
            result.Id,
            result.PartnerId,
            result.PartnerName,
            result.Slug,
            result.Title,
            result.Description,
            result.Status,
            result.CreatedByUserId,
            result.CreatedAt,
            result.UpdatedAt,
            PublicUrl = publicUrl
        });
    }
}