using FluentValidation.AspNetCore;

namespace ClubeBeneficios.Partners.Api.Extensions;

public static class ControllersExtensions
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation();

        services.AddEndpointsApiExplorer();

        return services;
    }
}
