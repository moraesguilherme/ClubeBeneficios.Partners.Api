using ClubeBeneficios.Partners.Api.Middlewares;

namespace ClubeBeneficios.Partners.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseUserContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();
        return app;
    }
}
