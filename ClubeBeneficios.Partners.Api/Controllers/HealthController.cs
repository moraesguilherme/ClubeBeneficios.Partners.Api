using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Partners.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            service = "ClubeBeneficios.Partners.Api",
            utcNow = DateTime.UtcNow
        });
    }
}
