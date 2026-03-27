using System.Net;
using System.Text.Json;

namespace ClubeBeneficios.Partners.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = JsonSerializer.Serialize(new
            {
                message = "Ocorreu um erro interno ao processar a requisiÃ§Ã£o.",
                detail = exception.Message
            });

            await context.Response.WriteAsync(payload);
        }
    }
}
