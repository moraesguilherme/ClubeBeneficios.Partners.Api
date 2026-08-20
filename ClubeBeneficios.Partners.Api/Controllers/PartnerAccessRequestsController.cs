using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClubeBeneficios.Partners.Domain.Dtos.Requests;
using ClubeBeneficios.Partners.Domain.Services;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/partners/public/access-requests")]
public class PartnerAccessRequestsController : ControllerBase
{
    private readonly IPartnerService _partnerService;

    public PartnerAccessRequestsController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePartnerAccessRequest request,
        CancellationToken cancellationToken)
    {
        var partnerRequest = new CreatePartnerRequest
        {
            TradeName = Normalize(request.TradeName),
            LegalName = Normalize(request.LegalName),
            Document = Normalize(request.Document),

            Email = Normalize(request.Email) ?? Normalize(request.ResponsibleEmail),
            Phone = Normalize(request.Phone) ?? Normalize(request.ResponsiblePhone),

            LogoUrl = null,
            Segment = Normalize(request.Segment),
            Category = Normalize(request.Category),
            ServiceRegion = Normalize(request.ServiceRegion),
            Website = Normalize(request.Website),
            Instagram = Normalize(request.Instagram),
            Description = Normalize(request.Description),

            Level = "bronze",
            IndicationFlowEnabled = true,
            AccessCodeFlowEnabled = true,

            OriginType = "self_signup",
            Status = "pending_review",

            ResponsibleName = Normalize(request.ResponsibleName),
            ResponsibleRole = Normalize(request.ResponsibleRole),
            ResponsibleEmail = Normalize(request.ResponsibleEmail),
            ResponsiblePhone = Normalize(request.ResponsiblePhone),

            CreatedByUserId = null,
            InitialNote = Normalize(request.InitialNote) ?? Normalize(request.Description)
        };

        var id = await _partnerService.CreateAsync(partnerRequest, cancellationToken);

        return Accepted(new
        {
            id,
            status = "pending_review",
            originType = "self_signup",
            message = "Solicitação recebida com sucesso. A equipe da Matilha irá analisar os dados enviados."
        });
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}